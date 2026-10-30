// Executar após gerar o HTML. Requer Playwright e Chrome ou Chromium instalado.
const {chromium}=require('playwright');
const assert=require('node:assert/strict');
const {pathToFileURL}=require('node:url');
const path=require('node:path');
(async()=>{
 const browser=await chromium.launch({headless:true,...(process.env.LOGUS_CHROME_PATH?{executablePath:process.env.LOGUS_CHROME_PATH}:{})});
 try{
  const page=await browser.newPage({viewport:{width:1920,height:1080}}),errors=[];
  page.on('pageerror',e=>errors.push(e.message));
  await page.goto(pathToFileURL(path.resolve(__dirname,'../prototipos/logus-desktop.html')).href);
  const f=page.frameLocator('iframe'),$=id=>f.locator('#'+id),nav=name=>f.locator('.lg-nav').getByRole('button',{name,exact:true}).click();
  const actor=id=>$('lg-session-user').selectOption(id);
  const request=async id=>{await nav('Solicitações');await $('lg-request-search').fill('');await f.locator('[data-screen=requests] [data-request-id="'+id+'"]').click();};
  const text=async(id,value)=>assert.match(await $(id).textContent(),value);
  await nav('Solicitações');await text('lg-request-search-result',/20 de 20/);
  assert.ok(await $('lg-issued-list').locator('button').count());
  const logout=await $('lg-logout').boundingBox();assert.ok(logout.y+logout.height<=1080);
  await $('lg-request-search').fill('Charlie');assert.equal(await f.locator('[data-screen=requests] [data-request-id]').count(),1);
  await $('lg-request-search').fill('000.000.000-02');assert.equal(await f.locator('[data-screen=requests] [data-request-id]').count(),1);

  // O professor pode alterar solicitação de outro professor; o autor real aparece.
  await actor('ana');await request('0042');
  await $('lg-message').fill('Conferência compartilhada de teste');await $('lg-message-send').click();await text('lg-events',/Ana Pereira/);
  const grades=f.locator('[data-module-average]');
  await grades.nth(0).fill('6.89');await grades.nth(1).fill('6.90');assert.equal(await $('lg-final-average').textContent(),'6,895');
  for(const [grade,expected,approved] of [['6.1','6,1',false],['6.8','6,8',false],['6.89','6,89',false],['6.9','7',true],['6.95','7',true],['7.9','8',true],['9.9','10',true],['6','6',false]]){
   for(const e of await grades.all())await e.fill(grade);
   await $('lg-attendance').fill('75');assert.equal(await $('lg-final-average').textContent(),expected);
   await $('lg-save').click();await $('lg-check').check();await $('lg-confirm').click();
   await text('lg-detail-status',approved?/Solicitações prontas/:/Em andamento/);
   // Reabrir por correção para a próxima iteração.
   if(approved){await grades.first().fill('5');await $('lg-save').click();await text('lg-detail-status',/Em andamento/);}
  }
  for(const e of await grades.all())await e.fill('6.95');
  await $('lg-attendance').fill('74.9');await $('lg-save').click();await $('lg-check').check();await $('lg-confirm').click();await text('lg-detail-status',/Em andamento/);
  await $('lg-attendance').fill('75');await $('lg-check').check();await $('lg-confirm').click();await text('lg-feedback',/Salve/);
  await $('lg-save').click();await $('lg-check').check();await $('lg-confirm').click();await text('lg-detail-status',/Solicitações prontas/);

  // Emissão externa e correção preservam o registro de emissão e avisam Certificados.
  await actor('cert');await request('0042');assert.equal(await grades.first().isDisabled(),true);
  await $('lg-mark-issued').click();await $('lg-action-ok').click();await text('lg-detail-status',/Emitido/);
  await actor('samuel');await request('0042');await grades.first().fill('8');await $('lg-save').click();await text('lg-detail-status',/Em andamento/);await text('lg-issuance-history',/Paula Mendes/);await text('lg-events',/6.95/);
  await actor('cert');await nav('Solicitações');await $('lg-notifications').locator('summary').click();await text('lg-notification-list',/Samuel Braz/);

  // Criação inicial não depende de médias; valida limites de interesses e duplo clique.
  await actor('samuel');await nav('Solicitações');await $('lg-new-request').click();
  const form=$('lg-student-form'),fields={name:'Aluno de Teste',cpf:'00000000888',birth:'2000-01-01',phone:'(11) 90000-0888',address:'Rua Teste, 1',district:'Centro',city:'São Paulo'};
  for(const [name,value] of Object.entries(fields))await form.locator('[name='+name+']').fill(value);
  await $('lg-student-save').click();await text('lg-form-error',/no mínimo 1/);
  const interests=form.locator('[name=interest]');for(let i=0;i<4;i++)await interests.nth(i).check();await $('lg-student-save').click();await text('lg-form-error',/no máximo 3/);
  for(let i=1;i<4;i++)await interests.nth(i).uncheck();
  await $('lg-student-save').evaluate(e=>{e.click();e.click();});await text('lg-request-search-result',/21 de 21/);
  await nav('Rematrículas');await $('lg-commercial-search').fill('00000000888');assert.equal(await $('lg-commercial-list').locator('button').count(),1);assert.equal(await $('lg-commercial-detail').isVisible(),false);
  await $('lg-commercial-list').locator('button').click();await text('lg-commercial-phone',/90000-0888/);

  // Aviso de CPF: abrir existente não cria nada; criar outra preserva o aluno e a primeira.
  await nav('Solicitações');await $('lg-new-request').click();await form.locator('[name=cpf]').fill('00000000888');await form.locator('[name=name]').focus();await $('lg-duplicate-dialog').waitFor({state:'visible'});await $('lg-duplicate-list').locator('button').click();await text('lg-student-title',/Aluno de Teste/);
  await nav('Solicitações');await text('lg-request-search-result',/21 de 21/);await $('lg-new-request').click();await form.locator('[name=cpf]').fill('00000000888');await form.locator('[name=name]').focus();await $('lg-duplicate-new').click();await interests.first().check();await $('lg-student-save').click();await text('lg-request-search-result',/22 de 22/);

  // Catálogo automático; adaptações individuais; cancelamento conserva o curso da tela.
  await $('lg-request-search').fill('00000000888');const id=await f.locator('[data-screen=requests] [data-request-id]').first().getAttribute('data-request-id');await request(id);
  await $('lg-course').selectOption('games');assert.equal(await f.locator('[data-module-name]').count(),5);
  await $('lg-course').selectOption('design');await $('lg-action-cancel').click();assert.equal(await $('lg-course').inputValue(),'games');
  await f.locator('.lg-module-row button').last().click();await $('lg-add-module').click();await f.locator('[data-module-name]').last().fill('Projeto personalizado');for(const e of await grades.all())await e.fill('8');await $('lg-attendance').fill('90');await $('lg-save').click();
  await actor('director');await nav('Cadastros');await $('lg-admin-course').selectOption('games');await $('lg-admin-course-modules').fill('Novo módulo padrão');await $('lg-save-course').click();
  await request(id);assert.equal(await f.locator('[data-module-name]').count(),5);assert.equal(await f.locator('[data-module-name]').last().inputValue(),'Projeto personalizado');
  await nav('Cadastros');await $('lg-admin-employee-name').fill('Professor Teste');await $('lg-admin-employee-cpf').fill('00000000889');await $('lg-admin-employee-phone').fill('(11) 90000-0889');await $('lg-save-employee').click();assert.match(await $('lg-session-user').textContent(),/Professor Teste/);

  // Comercial: ordem estável, sem seleção inicial, encerramento, reabertura e nova oportunidade.
  await actor('sales');await nav('Rematrículas');await $('lg-commercial-search').fill('');await f.locator('[data-commercial-filter=Todos]').click();assert.equal(await $('lg-commercial-detail').isVisible(),false);
  const dates=await $('lg-commercial-list').locator('button').evaluateAll(es=>es.map(e=>Date.parse(e.dataset.createdAt)));assert.deepEqual(dates,[...dates].sort((a,b)=>b-a));
  await $('lg-commercial-search').fill('00000000888');await $('lg-commercial-list').locator('button').first().click();await $('lg-commercial-status').selectOption('Recusou');await $('lg-note').fill('Não deseja agora');await $('lg-commercial-save').click();assert.equal(await $('lg-commercial-status').isDisabled(),true);
  await $('lg-commercial-reopen').click();await $('lg-action-ok').click();assert.equal(await $('lg-commercial-status').inputValue(),'Contatado');await text('lg-commercial-history',/Recusou → Contatado/);
  await $('lg-new-opportunity').click();await $('lg-opportunity-interests').locator('input').first().check();await $('lg-opportunity-save').click();await text('lg-commercial-notice',/anterior foi preservado/);await $('lg-commercial-search').fill('00000000888');assert.equal(await $('lg-commercial-list').locator('button').count(),3);
  await f.locator('[data-commercial-filter=Contatado]').click();assert.equal(await $('lg-commercial-list').locator('button').count(),1);
  await nav('Visão geral');await nav('Rematrículas');assert.equal(await $('lg-commercial-detail').isVisible(),false);

  // Layout e saída em resoluções menores, sem rolagem horizontal.
  for(const width of [1920,1366,1024,736,320]){
   await page.setViewportSize({width,height:1080});await actor('director');
   for(const name of ['Visão geral','Solicitações','Rematrículas','Cadastros']){await nav(name);assert.equal(await $('logus-draft').evaluate(()=>document.documentElement.scrollWidth>innerWidth),false,'Overflow '+width+' '+name);}
   await request('0042');assert.equal(await $('logus-draft').evaluate(()=>document.documentElement.scrollWidth>innerWidth),false,'Overflow detalhe '+width);await $('lg-edit-student').click();assert.equal(await $('logus-draft').evaluate(()=>document.documentElement.scrollWidth>innerWidth),false,'Overflow frente '+width);
  }
  await page.setViewportSize({width:1920,height:1080});await nav('Solicitações');await $('lg-logout').click();await $('lg-cpf').fill('usuario ficticio');await $('lg-password').fill('senha ficticia');await $('lg-login-enter').click();await f.locator('[data-screen=dashboard]').waitFor({state:'visible'});
  assert.deepEqual(errors,[]);console.log('OK: médias/limites, frequência, fluxo acadêmico/emissão/revisão, duplicidade, catálogo, autoria, cadastros, comercial, pesquisa, filtros, saída e layout em 5 larguras.');
 }finally{await browser.close();}
})().catch(e=>{console.error(e);process.exitCode=1;});
