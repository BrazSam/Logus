// Interações locais do rascunho. Não há backend nem persistência de dados pessoais.
(()=>{
const root=document.getElementById('logus-draft');
const get=id=>root.querySelector('#'+id);
const form=get('lg-student-form');
const fields=['name','cpf','birth','phone','guardian','address','district','city','requestDate','studentConfirmation'];
const labels={name:'nome',cpf:'CPF',birth:'data de nascimento',phone:'telefone',guardian:'responsável',address:'endereço',district:'bairro',city:'cidade',requestDate:'data da solicitação',studentConfirmation:'nome da confirmação'};
const today='2026-10-30';
const records=[
 {id:'0042',name:'Charlie Kirk',interests:['Design Digital'],phone:'(11) 90000-1234',cpf:'00000000001',birth:'2000-01-01',guardian:'',address:'Rua de Exemplo, 10',district:'Centro',city:'São Paulo',requestDate:today,studentConfirmation:'Charlie Kirk',studentChecked:true,modules:[{name:'Word',average:'8'},{name:'Excel',average:'7'}],attendance:'80',teacherDate:today,course:'Informática profissional',start:'2026-03-02',end:'2026-10-29',ready:false,commercialStatus:'Não contatado',note:'',events:[{text:'Preencheu os módulos e as médias.',type:'Histórico',time:'29/10/2026 · 14:20'}]},
 {id:'0041',name:'Jeffrey Epstein',interests:['Power BI'],phone:'(11) 90000-5678',cpf:'00000000002',birth:'2000-02-02',guardian:'',address:'Rua de Exemplo, 20',district:'Centro',city:'São Paulo',requestDate:today,studentConfirmation:'Jeffrey Epstein',studentChecked:true,modules:[{name:'Word',average:'8'},{name:'Excel',average:'8.5'}],attendance:'90',teacherDate:today,course:'Informática profissional',start:'2026-03-02',end:'2026-10-29',ready:true,commercialStatus:'Contatado',note:'',events:[{text:'Conferiu os dados e encaminhou para certificados.',type:'Histórico',time:'29/10/2026 · 10:25'}]}
];
records[0].createdAt='2026-10-29T09:10:00-03:00';records[0].requestDate='2026-10-29';
records[1].createdAt='2026-10-28T11:20:00-03:00';records[1].requestDate='2026-10-28';
const commercialStatuses=['Não contatado','Contatado','Rematriculado','Recusou'];
const demoStudents=[
 ['Ana Clara Souza','Design Digital'],['Pedro Henrique Lima','Power BI'],
 ['Mariana Costa','Assistente Administrativo'],['Lucas Oliveira','Desenvolvedor de Games'],
 ['Beatriz Santos','Edição de Vídeo'],['Gabriel Almeida','Desenho Mecânico e Arquitetônico'],
 ['Júlia Ferreira','Power BI'],['Rafael Martins','Design Digital'],
 ['Isabela Rocha','Assistente Administrativo'],['Matheus Ribeiro','Desenvolvedor de Games'],
 ['Camila Alves','Edição de Vídeo'],['Bruno Carvalho','Desenho Mecânico e Arquitetônico'],
 ['Larissa Gomes','Design Digital'],['Felipe Barbosa','Power BI'],
 ['Amanda Nascimento','Assistente Administrativo'],['João Victor Pereira','Desenvolvedor de Games'],
 ['Sofia Cardoso','Edição de Vídeo'],['Daniel Teixeira','Outros: Inteligência Artificial para o mercado']
];
demoStudents.forEach(([name,interest],index)=>{
 const createdAt=new Date(Date.parse('2026-10-29T17:45:00-03:00')-index*5*60*60*1000).toISOString();
 const requestDate=new Intl.DateTimeFormat('en-CA',{timeZone:'America/Sao_Paulo'}).format(new Date(createdAt));
 const commercialStatus=commercialStatuses[index%4],ready=index%3!==0,hasModules=ready||index%2===0;
 const notes={'Não contatado':'','Contatado':'Contato realizado; aguardando retorno.','Rematriculado':'Matrícula externa confirmada nesta demonstração.','Recusou':'Não deseja continuar os estudos neste momento.'};
 records.push({id:String(43+index).padStart(4,'0'),name,cpf:String(100+index).padStart(11,'0'),birth:'2000-01-01',phone:'(11) 90000-'+String(2000+index),guardian:'',address:'Rua de Exemplo, '+(30+index),district:'Centro',city:'São Paulo',requestDate,studentConfirmation:name,studentChecked:true,interests:[interest],modules:hasModules?[{name:'Word',average:'8'},{name:'Excel',average:String(7+index%3)}]:[],attendance:hasModules?'85':'',teacherDate:hasModules?requestDate:'',course:hasModules?'Informática profissional':'',start:'',end:'',ready,commercialStatus,note:notes[commercialStatus],createdAt,events:[{text:'Criou a solicitação com os dados conferidos do aluno e os interesses. Acompanhamento comercial adicionado automaticamente.',type:'Histórico',time:requestDate},...(ready?[{text:'Conferiu os dados e encaminhou para certificados.',type:'Histórico',time:requestDate}]:[])]});
});
function newestFirst(a,b){return Date.parse(b.createdAt)-Date.parse(a.createdAt)||Number(b.id)-Number(a.id);}
function createdLabel(record){return new Intl.DateTimeFormat('pt-BR',{timeZone:'America/Sao_Paulo',dateStyle:'short',timeStyle:'short'}).format(new Date(record.createdAt));}
let active=records[0],commercial=null,editing=null,loggedIn=true,photoUrl=null,commercialFilter='Todos';
const saved=window.openai?.widgetState?.modelContent;
function remember(page){window.openai?.setWidgetState?.({modelContent:{page,layout:'nova-solicitacao',loggedIn},privateContent:null})?.catch(()=>{});}
function show(page,persist=true){
 if(!loggedIn&&page!=='login')page='login';
 root.classList.toggle('lg-logged-out',page==='login');
 root.querySelectorAll('[data-screen]').forEach(e=>e.hidden=e.dataset.screen!==page);
 root.querySelectorAll('.lg-nav [data-page]').forEach(e=>e.setAttribute('aria-pressed',String(e.dataset.page===(['student','detail'].includes(page)?'requests':page))));
 if(page==='requests')list();if(page==='dashboard')dashboard();if(page==='commercial'){commercial=null;get('lg-commercial-notice').textContent='';renderCommercial();}
 if(persist){remember(page);window.scrollTo(0,0);}
}
function text(tag,value,className){const e=document.createElement(tag);e.textContent=value;if(className)e.className=className;return e;}
function recordButton(record,callback){const b=document.createElement('button');b.type='button';b.className='lg-student';b.append(text('strong',record.name),text('span',(record.course||'Curso a informar pelo professor')+' · #'+record.id,'lg-small'));b.onclick=callback;return b;}
function status(record){return record.ready?'Pronta para certificados':!record.modules.length||record.modules.some(m=>!m.name||m.average==='')?'Aguardando médias':'Aguardando conferência';}
function normalize(value){return String(value||'').normalize('NFD').replace(/[\u0300-\u036f]/g,'').toLocaleLowerCase('pt-BR').trim();}
function matches(record,query){const q=normalize(query);if(!q)return true;if(/^[\d.\s/#-]+$/.test(q)){const digits=q.replace(/\D/g,'');return digits!==''&&[record.cpf,record.id].some(value=>String(value||'').replace(/\D/g,'').includes(digits));}const haystack=normalize([record.name,record.cpf,record.id].join(' '));return q.split(/\s+/).every(term=>haystack.includes(term));}
function list(){const filtered=records.filter(r=>matches(r,get('lg-request-search').value));get('lg-request-search-result').textContent=filtered.length+' de '+records.length+' solicitações';['pending','ready'].forEach(queue=>{
 const target=get('lg-'+queue+'-list');target.replaceChildren();const items=filtered.filter(r=>r.ready===(queue==='ready'));get('lg-'+queue+'-count').textContent=items.length;
 items.forEach(r=>{const b=recordButton(r,()=>open(r));b.append(text('span',status(r),'lg-status'),text('span','Prof. Samuel Braz · Abrir solicitação →','lg-student-footer'));target.append(b);});
 if(!items.length)target.append(text('p',get('lg-request-search').value.trim()?'Nenhuma solicitação encontrada nesta fila.':'Nenhuma solicitação nesta fila.','lg-small'));
});}
function dashboard(){get('lg-stat-pending').textContent=records.filter(r=>!r.ready).length;get('lg-stat-ready').textContent=records.filter(r=>r.ready).length;get('lg-stat-commercial').textContent=records.filter(r=>r.commercialStatus==='Não contatado').length;const target=get('lg-dashboard-pending');target.replaceChildren();records.filter(r=>!r.ready).forEach(r=>target.append(recordButton(r,()=>open(r))));if(!target.childElementCount)target.append(text('p','Nenhuma solicitação aguardando o professor.','lg-small'));}
function renderEvents(){get('lg-events').replaceChildren();active.events.forEach(event=>{const li=document.createElement('li');li.append(text('strong','Samuel Braz'),text('p',event.text),text('span',event.time+' · '+event.type,'lg-small'));get('lg-events').append(li);});}
function entry(message,type='Alteração automática'){active.events.push({text:message,type,time:'Agora'});renderEvents();}
function open(record){
 active=record;get('lg-student-title').textContent=record.name+' · #'+record.id;get('lg-detail-name').textContent=record.name;get('lg-detail-phone').textContent=record.phone;get('lg-detail-interests').textContent=record.interests.join(', ');
 ['course','start','end','attendance'].forEach(field=>get('lg-'+field).value=record[field]);get('lg-teacher-date').value=record.teacherDate||today;
 const personal=get('lg-detail-personal');personal.replaceChildren();['cpf','birth','guardian','address','district','city','requestDate','studentConfirmation'].forEach(field=>{const item=document.createElement('div');item.append(text('dt',labels[field]),text('dd',record[field]||'Não informado'));personal.append(item);});
 renderModules(record.modules);updateAverage();
 get('lg-detail-status').textContent=status(record);get('lg-check').checked=record.ready;get('lg-confirm').disabled=true;get('lg-confirm').textContent=record.ready?'Encaminhada para certificados':'Confirmar e encaminhar';get('lg-feedback').textContent='A emissão do certificado acontece fora do Logus.';get('lg-message').value='';renderEvents();show('detail');
}
function otherState(){const enabled=get('lg-other-check').checked;get('lg-other-label').hidden=!enabled;get('lg-other-interest').disabled=!enabled;get('lg-other-interest').required=enabled;}
function studentForm(record=null){editing=record;form.reset();get('lg-form-error').textContent='';get('lg-form-title').textContent=record?'Editar dados do aluno':'Nova solicitação de certificado';get('lg-student-save').textContent=record?'Salvar alterações':'Salvar solicitação';fields.forEach(field=>form.elements.namedItem(field).value=record?.[field]||(field==='requestDate'?today:''));form.elements.namedItem('studentChecked').checked=record?.studentChecked||false;form.querySelectorAll('[name=interest]').forEach(e=>e.checked=record?.interests.includes(e.value)||false);const other=record?.interests.find(i=>i.startsWith('Outros: '));get('lg-other-check').checked=!!other;get('lg-other-interest').value=other?other.slice(8):'';otherState();show('student');}
function saveStudent(){
 get('lg-form-error').textContent='';if(!form.reportValidity())return;
 const interests=[...form.querySelectorAll('[name=interest]:checked')].map(e=>e.value==='Outros'?'Outros: '+get('lg-other-interest').value.trim():e.value);
 if(!interests.length){get('lg-form-error').textContent='Escolha pelo menos um curso de interesse.';return;}
 const values=Object.fromEntries(fields.map(field=>[field,form.elements.namedItem(field).value.trim()]));
 if(!values.name||!values.phone||!values.address||!values.district||!values.city||!values.studentConfirmation){get('lg-form-error').textContent='Preencha os campos obrigatórios.';return;}
 if(get('lg-other-check').checked&&!get('lg-other-interest').value.trim()){get('lg-form-error').textContent='Descreva a certificação em Outros.';return;}
 const cpf=values.cpf.replace(/\D/g,'');if(cpf.length!==11){get('lg-form-error').textContent='Informe os 11 dígitos do CPF (validação de formato no rascunho).';return;}
 if(records.some(r=>r!==editing&&r.cpf?.replace(/\D/g,'')===cpf)){get('lg-form-error').textContent='Já existe uma solicitação com esse CPF nesta demonstração.';return;}
 if(editing){
  active=editing;fields.forEach(field=>{if((active[field]||'')!==values[field])entry('Alterou '+labels[field]+' de “'+(active[field]||'não informado')+'” para “'+values[field]+'”.');});
  if(active.interests.join('|')!==interests.join('|'))entry('Alterou interesses de “'+active.interests.join(', ')+'” para “'+interests.join(', ')+'”.');
  Object.assign(active,values,{interests,studentChecked:true});editing=null;open(active);
 }else{
  const id=String(Math.max(...records.map(r=>Number(r.id)))+1).padStart(4,'0');
  records.push({...values,id,interests,studentChecked:true,modules:[],attendance:'',teacherDate:'',course:'',start:'',end:'',ready:false,commercialStatus:'Não contatado',note:'',createdAt:new Date().toISOString(),events:[{text:'Registrou a conferência dos dados do aluno por '+values.studentConfirmation+' em '+values.requestDate+'. Criou o acompanhamento comercial automaticamente.',type:'Histórico',time:'Agora'}]});
  get('lg-request-search').value='';get('lg-request-feedback').textContent='Solicitação de '+values.name+' criada em Em andamento. Aluno adicionado automaticamente em Rematrículas, sem notas.';show('requests');
 }
}
function statusKey(value){return {'Não contatado':'uncontacted','Contatado':'contacted','Rematriculado':'enrolled','Recusou':'declined'}[value];}
function statusBadge(value){const badge=text('span',value,'lg-commercial-badge');badge.dataset.status=statusKey(value);return badge;}
function renderCommercialList(){
 const searched=records.filter(r=>matches(r,get('lg-commercial-search').value));
 root.querySelectorAll('[data-commercial-filter]').forEach(button=>{const value=button.dataset.commercialFilter;button.setAttribute('aria-pressed',String(commercialFilter===value));button.querySelector('[data-filter-count]').textContent=value==='Todos'?searched.length:searched.filter(r=>r.commercialStatus===value).length;});
 const filtered=searched.filter(r=>commercialFilter==='Todos'||r.commercialStatus===commercialFilter).sort(newestFirst),list=get('lg-commercial-list');
 get('lg-commercial-search-result').textContent=filtered.length+' de '+records.length+' alunos · '+(commercialFilter==='Todos'?'Todas as situações':commercialFilter);
 list.replaceChildren();filtered.forEach(r=>{const b=recordButton(r,()=>{commercial=r;renderCommercial();});b.dataset.createdAt=r.createdAt;b.dataset.commercialStatus=r.commercialStatus;b.dataset.recordId=r.id;b.setAttribute('aria-pressed',String(commercial===r));b.append(text('span','Interesses: '+r.interests.join(', '),'lg-small'));const footer=document.createElement('span');footer.className='lg-commercial-card-footer';footer.append(statusBadge(r.commercialStatus),text('span','Criado em '+createdLabel(r),'lg-small'));b.append(footer);list.append(b);});
 if(!filtered.length)list.append(text('p','Nenhum aluno encontrado para esta pesquisa e situação.','lg-small'));
 const selectedVisible=filtered.includes(commercial);get('lg-commercial-detail').hidden=!selectedVisible;get('lg-commercial-select-prompt').hidden=selectedVisible||!filtered.length;
}
function renderCommercial(){
 renderCommercialList();
 if(!commercial)return;
 get('lg-commercial-name').textContent=commercial.name;get('lg-commercial-current-status').replaceChildren(statusBadge(commercial.commercialStatus));get('lg-commercial-interests').replaceChildren(...commercial.interests.map(interest=>text('li',interest)));get('lg-commercial-phone').textContent=commercial.phone;get('lg-commercial-origin').textContent='Professor: Samuel Braz · Origem: solicitação #'+commercial.id+' · '+status(commercial)+' · Criado em '+createdLabel(commercial);get('lg-commercial-status').value=commercial.commercialStatus;get('lg-note').value=commercial.note;get('lg-commercial-feedback').textContent='';
}
const academicFields={modules:'módulos e médias',course:'curso concluído',start:'data de início',end:'data de término',attendance:'frequência (%)',teacherDate:'data da conferência do professor'};
function renderModules(modules){const rows=get('lg-module-rows');rows.replaceChildren();for(let i=0;i<13;i++){const row=document.createElement('div');row.className='lg-module-row';row.dataset.moduleIndex=i;const name=document.createElement('input'),average=document.createElement('input');name.id='lg-module-name-'+i;name.setAttribute('aria-label','Módulo '+(i+1));name.placeholder='Módulo '+(i+1);name.maxLength=120;name.value=modules[i]?.name||'';average.id='lg-module-average-'+i;average.setAttribute('aria-label','Média '+(i+1));average.type='number';average.min='0';average.max='10';average.step='0.01';average.placeholder='—';average.value=modules[i]?.average??'';row.append(name,average);rows.append(row);}}
function readModules(){return [...root.querySelectorAll('.lg-module-row')].map(row=>({name:row.children[0].value.trim(),average:row.children[1].value})).filter(m=>m.name||m.average!=='');}
function mean(modules){if(!modules.length||modules.some(m=>!m.name||m.average===''||!Number.isFinite(Number(m.average))||Number(m.average)<0||Number(m.average)>10))return null;return Math.round(modules.reduce((sum,m)=>sum+Number(m.average),0)/modules.length*100)/100;}
function updateAverage(){const modules=readModules(),average=mean(modules),frequency=get('lg-attendance').value;get('lg-final-average').textContent=average===null?'—':average.toLocaleString('pt-BR',{minimumFractionDigits:2,maximumFractionDigits:2});get('lg-eligibility').textContent=average===null||frequency===''||!get('lg-attendance').checkValidity()?'Preencha médias de 0 a 10 e frequência de 0 a 100% para conferir os critérios.':average>=7&&Number(frequency)>=75?'Atende aos critérios informados no formulário.':'Ainda não atende aos critérios informados no formulário.';}
function academicValues(){return {modules:readModules(),course:get('lg-course').value.trim(),start:get('lg-start').value,end:get('lg-end').value,attendance:get('lg-attendance').value,teacherDate:get('lg-teacher-date').value};}
function draftError(values){if(values.modules.some(m=>m.average!==''&&(!m.name||!Number.isFinite(Number(m.average))||Number(m.average)<0||Number(m.average)>10))||[...root.querySelectorAll('.lg-module-row input[type=number]')].some(e=>e.validity.badInput))return 'Informe o nome do módulo e uma média entre 0 e 10.';if(values.attendance!==''&&(!get('lg-attendance').checkValidity()||Number(values.attendance)<0||Number(values.attendance)>100))return 'Informe frequência entre 0 e 100%.';if(values.start&&values.end&&values.start>values.end)return 'A data final do curso deve ser igual ou posterior à inicial.';return '';}
function academicValid(values){return !draftError(values)&&values.course&&values.teacherDate&&values.modules.length&&values.modules.every(m=>m.name&&m.average!=='')&&mean(values.modules)>=7&&values.attendance!==''&&Number(values.attendance)>=75&&active.studentChecked;}
function sameAcademic(values){return Object.entries(values).every(([field,value])=>JSON.stringify(active[field])===JSON.stringify(value));}
function formatAcademic(field,value){if(field==='modules')return value.length?value.map(m=>m.name+': '+(m.average||'sem média')).join('; '):'não informado';return value||'não informado';}
root.querySelectorAll('[data-page]').forEach(b=>b.onclick=()=>show(b.dataset.page));
get('lg-request-search').addEventListener('input',list);
get('lg-commercial-search').addEventListener('input',renderCommercialList);
root.querySelectorAll('[data-commercial-filter]').forEach(button=>button.onclick=()=>{commercialFilter=button.dataset.commercialFilter;renderCommercialList();});
get('lg-new-request').onclick=()=>studentForm();get('lg-edit-student').onclick=()=>studentForm(active);
get('lg-student-back').onclick=get('lg-form-cancel').onclick=()=>editing?open(editing):show('requests');
get('lg-student-save').onclick=saveStudent;form.onsubmit=e=>{e.preventDefault();saveStudent();};
get('lg-other-check').onchange=otherState;
get('lg-back').onclick=()=>show('requests');
get('lg-check').onchange=()=>get('lg-confirm').disabled=!get('lg-check').checked||active.ready;
get('lg-save').onclick=()=>{const values=academicValues(),error=draftError(values);if(error){get('lg-feedback').textContent=error;return;}let changed=false;Object.entries(values).forEach(([field,value])=>{if(JSON.stringify(active[field])!==JSON.stringify(value)){entry('Alterou '+academicFields[field]+' de “'+formatAcademic(field,active[field])+'” para “'+formatAcademic(field,value)+'”.');changed=true;}});Object.assign(active,values);get('lg-check').checked=false;get('lg-confirm').disabled=true;get('lg-detail-status').textContent=status(active);get('lg-feedback').textContent=changed?'Dados do professor salvos. Você pode continuar o preenchimento depois. O certificado externo não é alterado automaticamente.':'Nenhuma alteração nos dados do professor.';};
get('lg-confirm').onclick=()=>{if(!get('lg-check').checked||active.ready)return;const values=academicValues();if(!academicValid(values)){get('lg-feedback').textContent='Para encaminhar, informe o curso, a data da conferência, os módulos e suas médias. É necessário média final ≥ 7 e frequência ≥ 75%, conforme o formulário.';return;}if(!sameAcademic(values)){get('lg-feedback').textContent='Salve as alterações do professor antes de confirmar.';return;}active.ready=true;entry('Samuel Braz confirmou a conferência em '+values.teacherDate+' e encaminhou para certificados. Média final: '+mean(values.modules).toLocaleString('pt-BR')+'; frequência: '+values.attendance+'%.','Histórico');get('lg-detail-status').textContent=status(active);get('lg-confirm').disabled=true;get('lg-confirm').textContent='Encaminhada para certificados';get('lg-feedback').textContent='Solicitação encaminhada. Ao voltar, ela estará na fila da direita.';};
root.querySelector('[data-screen=detail]').addEventListener('input',e=>{if(e.target.closest('#lg-module-rows')||['lg-course','lg-start','lg-end','lg-attendance','lg-teacher-date'].includes(e.target.id)){updateAverage();get('lg-check').checked=false;get('lg-confirm').disabled=true;}});
function sendMessage(){const input=get('lg-message');if(!input.value.trim())return;entry(input.value.trim(),'Mensagem');input.value='';}
get('lg-message-send').onclick=sendMessage;get('lg-message-form').onsubmit=e=>{e.preventDefault();sendMessage();};
get('lg-commercial-save').onclick=()=>{if(!commercial)return;commercial.commercialStatus=get('lg-commercial-status').value;commercial.note=get('lg-note').value;renderCommercialList();get('lg-commercial-current-status').replaceChildren(statusBadge(commercial.commercialStatus));get('lg-commercial-notice').textContent='Acompanhamento de '+commercial.name+' salvo como '+commercial.commercialStatus+'.';get('lg-commercial-feedback').textContent='Acompanhamento salvo nesta demonstração. Nenhuma matrícula foi realizada pelo Logus.';};
get('lg-logout').onclick=()=>{loggedIn=false;get('lg-login-form').reset();show('login');};
function login(){if(!get('lg-login-form').reportValidity())return;loggedIn=true;get('lg-login-form').reset();show('dashboard');}
get('lg-login-enter').onclick=login;get('lg-login-form').onsubmit=e=>{e.preventDefault();login();};get('lg-login-form').onkeydown=e=>{if(e.key==='Enter'){e.preventDefault();login();}};
get('lg-photo-button').onclick=()=>get('lg-photo-input').click();get('lg-photo-input').onchange=()=>{const file=get('lg-photo-input').files[0];if(!file||!['image/png','image/jpeg','image/webp'].includes(file.type))return;if(photoUrl)URL.revokeObjectURL(photoUrl);photoUrl=URL.createObjectURL(file);get('lg-photo').src=photoUrl;get('lg-photo').hidden=false;get('lg-initials').hidden=true;};
if(saved?.layout==='nova-solicitacao')loggedIn=saved.loggedIn!==false;
show(saved?.layout==='nova-solicitacao'&&['dashboard','requests','commercial','login'].includes(saved.page)?saved.page:'dashboard',false);
window.addEventListener('openai:set_globals',e=>{const state=e.detail?.globals?.widgetState?.modelContent;if(state?.layout==='nova-solicitacao'&&['dashboard','requests','commercial','login'].includes(state.page)){loggedIn=state.loggedIn!==false;show(state.page,false);}});
})();


