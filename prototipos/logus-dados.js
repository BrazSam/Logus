// Dados fictícios para a demonstração.
globalThis.LogusSeed=(()=>{
const today='2026-10-30';
const records=[
 {id:'0042',name:'Charlie Kirk',interests:['Design Digital'],phone:'(11) 90000-1234',cpf:'00000000001',birth:'2000-01-01',guardian:'',address:'Rua de Exemplo, 10',district:'Centro',city:'São Paulo',requestDate:today,modules:[{name:'Word',average:'8'},{name:'Excel',average:'7'}],attendance:'80',teacherDate:today,course:'Informática profissional',start:'2026-03-02',end:'2026-10-29',ready:false,commercialStatus:'Não contatado',note:'',events:[{text:'Preencheu os módulos e as médias.',type:'Histórico',time:'29/10/2026 · 14:20'}]},
 {id:'0041',name:'Jeffrey Epstein',interests:['Power BI'],phone:'(11) 90000-5678',cpf:'00000000002',birth:'2000-02-02',guardian:'',address:'Rua de Exemplo, 20',district:'Centro',city:'São Paulo',requestDate:today,modules:[{name:'Word',average:'8'},{name:'Excel',average:'8.5'}],attendance:'90',teacherDate:today,course:'Informática profissional',start:'2026-03-02',end:'2026-10-29',ready:true,commercialStatus:'Contatado',note:'',events:[{text:'Conferiu os dados e encaminhou para certificados.',type:'Histórico',time:'29/10/2026 · 10:25'}]}
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
 records.push({id:String(43+index).padStart(4,'0'),name,cpf:String(100+index).padStart(11,'0'),birth:'2000-01-01',phone:'(11) 90000-'+String(2000+index),guardian:'',address:'Rua de Exemplo, '+(30+index),district:'Centro',city:'São Paulo',requestDate,interests:[interest],modules:hasModules?[{name:'Word',average:'8'},{name:'Excel',average:String(7+index%3)}]:[],attendance:hasModules?'85':'',teacherDate:hasModules?requestDate:'',course:hasModules?'Informática profissional':'',start:'',end:'',ready,commercialStatus,note:notes[commercialStatus],createdAt,events:[{text:'Criou a solicitação com os dados conferidos do aluno e os interesses. Acompanhamento comercial adicionado automaticamente.',type:'Histórico',time:requestDate},...(ready?[{text:'Conferiu os dados e encaminhou para certificados.',type:'Histórico',time:requestDate}]:[])]});
});

return records;
})();
