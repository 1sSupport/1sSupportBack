webpackJsonp([1],{"3+es":function(t,e){},"54Sm":function(t,e){},"7ZZq":function(t,e){},"8aC8":function(t,e){},D6cl:function(t,e){},D7I9:function(t,e){},Jp84:function(t,e){},NHnr:function(t,e,s){"use strict";Object.defineProperty(e,"__esModule",{value:!0});var i=s("7+uW"),a=s("mtWM"),r=s.n(a),o={name:"ModalComponent",props:{token:{required:!0,type:String},sessionId:{required:!0,type:Number}},data:()=>({dialog:!1,supportRequestTelephone:"",supportRequestText:"",supportRequestTitle:""}),methods:{closeAndSend:async function(){this.dialog=!1;let t={method:"post",url:"http://www.u0612907.plsk.regruhosting.ru/api/Session/CreateSupportMessage",headers:{Authorization:"Bearer "+this.token},data:{ContactData:this.supportRequestTelephone,SessionID:this.sessionId,Text:this.supportRequestText,Title:this.supportRequestTitle}};var e=await r()(t);console.log(e)}}},n={render:function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("v-container",[s("v-layout",{attrs:{row:""}},[s("v-dialog",{attrs:{"max-width":"700"},model:{value:t.dialog,callback:function(e){t.dialog=e},expression:"dialog"}},[s("p",{staticClass:"activ",attrs:{slot:"activator",persistent:"",maxwidth:"100px",color:"grey",dark:""},slot:"activator"},[t._v("Я не нашел подоходящей статьи")]),t._v(" "),s("v-card",[s("div",{staticClass:"header-logo"},[s("v-flex",{attrs:{xs3:""}},[s("v-img",{staticClass:"v-img-logo",attrs:{src:"https://argos1c.ru/templates/argos/images/logo.png"}})],1)],1),t._v(" "),s("v-card-text",[s("v-container",{attrs:{"grid-list-md":""}},[s("v-layout",{attrs:{wrap:""}},[s("v-flex",{attrs:{xs12:""}},[s("v-text-field",{attrs:{label:"Контактный телефон"},model:{value:t.supportRequestTelephone,callback:function(e){t.supportRequestTelephone=e},expression:"supportRequestTelephone"}})],1),t._v(" "),s("v-flex",{attrs:{xs12:""}},[s("v-select",{attrs:{"append-icon":null,clearable:"",items:["Тема 1","Тема 2","Тема 3","Тема 4"],label:"Тема обращения",required:""},model:{value:t.supportRequestTitle,callback:function(e){t.supportRequestTitle=e},expression:"supportRequestTitle"}})],1),t._v(" "),s("v-flex",{attrs:{xs12:""}},[s("v-textarea",{attrs:{rows:"3",label:"Пожалуйста, опишите вашу проблему и мы свяжемся с вами",required:""},model:{value:t.supportRequestText,callback:function(e){t.supportRequestText=e},expression:"supportRequestText"}})],1)],1)],1),t._v(" "),s("small")],1),t._v(" "),s("v-card-actions",[s("v-spacer"),t._v(" "),s("v-btn",{staticClass:"v-btn-save",attrs:{color:"#3f66b2"},on:{click:function(e){t.closeAndSend()}}},[t._v("ОТПРАВИТЬ ЗАЯВКУ")])],1)],1)],1)],1)],1)},staticRenderFns:[]};var l=s("VU/8")(o,n,!1,function(t){s("D6cl")},"data-v-0a29b5fa",null).exports,u={render:function(){var t=this.$createElement,e=this._self._c||t;return e("v-card",{attrs:{flat:""}},[e("div",{staticClass:"header-logo"},[e("v-flex",{attrs:{xs3:""}},[e("v-img",{staticClass:"v-img-logo",attrs:{src:"https://argos1c.ru/templates/argos/images/logo.png"}})],1)],1)])},staticRenderFns:[]};var c={name:"App",components:{ModalComponent:l,Toolbar:s("VU/8")(null,u,!1,function(t){s("Jp84")},"data-v-33b73a61",null).exports}},p={render:function(){var t=this.$createElement,e=this._self._c||t;return e("div",{attrs:{id:"app"}},[e("toolbar"),this._v(" "),e("router-view")],1)},staticRenderFns:[]};var h=s("VU/8")(c,p,!1,function(t){s("3+es"),s("D7I9")},null,null).exports,d=s("/ocq"),v={name:"SearchInput",props:{sessionId:{required:!1,type:String}},data:()=>({searchString:"",usersView:!0,showHints:!1,shownHint:[],overlapOldHint:[],overlapNewHint:[],allHint:[{id:1,text:"иностранцы"},{id:2,text:"налоги"},{id:3,text:"иностранцы1"},{id:4,text:"иииии"},{id:5,text:"иноаааа"},{id:6,text:"налоги78888"}]}),mounted(){this.overlapNewHint=this.allHint},methods:{searchRequest:function(t){this.$emit("search",t)},PickHint(t){this.searchString=t,this.showHints=!1},HintSelections(t){this.overlapOldHint=this.overlapNewHint,this.overlapNewHint=[];for(var e=0;e<this.overlapOldHint.length;e++)this.overlapOldHint[e].text.startsWith(t)&&(this.overlapNewHint.push(this.overlapOldHint[e]),console.log(this.overlapOldHint[e]));this.overlapNewHint.length>=5?this.shownHint=this.overlapNewHint.slice(0,5):this.shownHint=this.overlapNewHint.slice(0,this.overlapNewHint.length),null!=t&&0!=t.length?this.showHints=!0:(this.showHints=!1,this.overlapNewHint=this.allHint)},GetArticles(t){}}},m={render:function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("v-container",[s("v-layout",[s("v-flex",{attrs:{xs12:""}},[s("v-card",{attrs:{flat:"",tile:""}},[s("v-card-title",{attrs:{"primary-title":""}},[s("h1",[t._v("Введите ключевые слова, описывающие проблему")])]),t._v(" "),s("v-card-actions",[s("v-layout",[s("v-flex",{attrs:{xs8:""}},[s("v-text-field",{attrs:{label:"Search",solo:"",clearable:"","hide-details":""},on:{input:function(e){t.HintSelections(t.searchString)},change:function(e){t.searchRequest(e)}},model:{value:t.searchString,callback:function(e){t.searchString=e},expression:"searchString"}}),t._v(" "),s("v-list",t._l(t.shownHint,function(e){return t.showHints?s("v-list-tile",{key:e.id,on:{click:function(s){t.PickHint(e.text)}}},[s("v-list-tile-content",[s("v-list-tile-title",{domProps:{innerHTML:t._s(e.text)}})],1)],1):t._e()}),1)],1),t._v(" "),s("v-flex",{attrs:{xs2:""}},[s("v-btn",{staticStyle:{height:"47px"},attrs:{color:"#3f66b2"},nativeOn:{click:function(e){e.stopPropagation(),t.GetArticles(t.searchString)}}},[s("v-icon",{attrs:{dark:""}},[t._v("search")])],1)],1)],1)],1)],1)],1)],1)],1)},staticRenderFns:[]};var b=s("VU/8")(v,m,!1,function(t){s("qrsy")},null,null).exports,g={name:"SearchResult",props:{token:{required:!0,type:String},searchResponse:{required:!1,type:Array},sessionId:{required:!1,type:Number},lastQuery:{required:!0,type:String}},data:()=>({totalItems:3,items:[{title:"Статья 1",subtitle:"<span class='text--primary'>Посадил дед репку. Выросла репка большая-пребольшая. Стал дед репку из земли тащить: тянет-потянет, вытянуть не может. Позвал дед бабку. Бабка за дедку, дедка за репку — тянут-потянут, вытянуть не могут. Позвала бабка внучку. Внучка за бабку, бабка за дедку, дедка за репку — тянут-потянут, вытянуть не могут."},{divider:!0,inset:!0},{title:"Статья 2 статья 2",subtitle:"Кликнула внучка Жучку. Жучка за внучку, внучка за бабку, бабка за дедку, дедка за репку — тянут-потянут, вытянуть не могут. Кликнула Жучка кошку. Кошка за Жучку, Жучка за внучку, внучка за бабку, бабка за дедку, дедка за репку — тянут-потянут, вытянуть не могут. Позвала кошка мышку. Мышка за кошку, кошка за Жучку, Жучка за внучку, внучка за бабку, бабка за дедку, дедка за репку — тянут-потянут, вытащили репку!"},{divider:!0,inset:!0},{title:"Статья 3 статья 3 статья 3 статья 3",subtitle:"Кликнула внучка Жучку. Жучка за внучку, внучка за бабку, бабка за дедку, дедка за репку — тянут-потянут, вытянуть не могут. Кликнула Жучка кошку. Кошка за Жучку, Жучка за внучку, внучка за бабку, бабка за дедку, дедка за репку — тянут-потянут, вытянуть не могут. Позвала кошка мышку. Мышка за кошку, кошка за Жучку, Жучка за внучку, внучка за бабку, бабка за дедку, дедка за репку — тянут-потянут, вытащили репку!"},{divider:!0,inset:!0}]})},q={render:function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("v-container",[s("v-layout",{attrs:{row:""}},[s("v-flex",{attrs:{xs12:""}},[0!=t.searchResponse?s("v-card",{staticClass:"text-xs-left",attrs:{flat:""}},[t._v("Результатов найдено: "+t._s(t.searchResponse.length))]):t._e(),t._v(" "),s("v-card",{attrs:{flat:"","max-width":"90%"}},[s("v-list",{attrs:{"three-line":""}},[t._l(this.searchResponse,function(e){return 0!=t.searchResponse?s("item",{key:e.id},[s("v-list-tile-content",[s("router-link",{staticClass:"article-title",attrs:{to:{name:"ArticlePage",params:{articleId:e.id,token:t.token,sessionId:t.sessionId,query:t.lastQuery}}}},[s("v-list-tile-title",{domProps:{innerHTML:t._s(e.title)}})],1),t._v(" "),s("v-list-tile-sub-title",{staticClass:"article-preview"},[t._v(t._s(e.text))])],1)],1):t._e()}),t._v(" "),t._l(t.items,function(e){return 0==t.searchResponse.length?s("item",{key:e.title},[s("v-list-tile-content",[s("router-link",{staticClass:"article-title",attrs:{to:{name:"ArticlePage"}}},[s("v-list-tile-title",{domProps:{innerHTML:t._s(e.title)}})],1),t._v(" "),s("v-list-tile-sub-title",{staticClass:"article-preview",domProps:{innerHTML:t._s(e.subtitle)}})],1)],1):t._e()})],2)],1),t._v(" "),0==t.searchResponse.length?s("v-pagination",{attrs:{length:4,"prev-icon":"mdi-menu-left","next-icon":"mdi-menu-right"},model:{value:t.items,callback:function(e){t.items=e},expression:"items"}}):t._e()],1)],1)],1)},staticRenderFns:[]};var f={name:"MainView",components:{SearchInput:b,SearchResult:s("VU/8")(g,q,!1,function(t){s("54Sm")},"data-v-139efdb8",null).exports,ModalComponent:l},data:()=>({token:"",sessionId:"",searchRequestResponse:[],lastQuery:""}),methods:{getToken:async function(){return(await r.a.post("http://www.u0612907.plsk.regruhosting.ru/api/Token/CreateToken",{inn:"000000000000",login:"test"})).data.access_token},startSession:async function(t){let e={method:"post",url:"http://www.u0612907.plsk.regruhosting.ru/api/Session/StartSession",headers:{Authorization:"Bearer "+t}};return(await r()(e)).data.sessionId},getArticles:async function(t,e){let s={method:"get",url:"http://www.u0612907.plsk.regruhosting.ru/api/Article/GetArticlesByQuery",headers:{Authorization:"Bearer "+this.token},params:{query:t,sessionId:e}};return(await r()(s)).data},searchHandler:async function(t){this.lastQuery=t,this.searchRequestResponse=await this.getArticles(t,this.sessionId)}},created(){(async()=>{this.token=await this.getToken(),this.sessionId=await this.startSession(this.token)})()}},_={render:function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("div",[s("search-input",{on:{search:function(e){t.searchHandler(e)}}}),t._v(" "),s("search-result",{attrs:{searchResponse:t.searchRequestResponse,token:t.token,sessionId:t.sessionId,lastQuery:t.lastQuery}}),t._v(" "),s("modal-component",{attrs:{token:t.token,sessionId:t.sessionId}})],1)},staticRenderFns:[]};var x=s("VU/8")(f,_,!1,function(t){s("8aC8"),s("qU6w")},null,null).exports,k={name:"ArticlePage",components:{ModalComponent:l},props:{token:{required:!0,type:String},sessionId:{required:!1,type:Number},articleId:{required:!0,type:Number},query:{required:!0,type:String}},data:()=>({article:"",example:'<div class="header">ЕГАИС</div><div id="actinfo"><p>Дата публикации 18.02.2016</p></div><h2>ЕГАИС при оптовой торговле алкоголем</h2><h3>Схема работы с ЕГАИС при оптовой торговле алкогольной продукцией</h3><p>Оптовые продавцы алкогольной продукции должны отражать в ЕГАИС факты и объемы закупок алкоголя у производителя или поставщика, а также его дальнейшую реализацию. </p><p>Схема работы оптового склада с маркируемым алкоголем упрощенно выглядит так:</p><ol><li>производитель (оптовый поставщик, импортер) при отгрузке товара заводит товарно-транспортную накладную (далее - ТТН) в программу &quot;1С&quot;, далее ТТН передается в электронном виде в ЕГАИС через Универсальный транспортный модуль (далее - УТМ);</li><li>оптовый покупатель (магазин) при поступлении товара получает из ЕГАИС электронную ТТН через УТМ;</li><li>оптовый покупатель (магазин) проверяет фактическое наличие поступившего алкоголя с данными в ТТН из ЕГАИС (поштучного либо партионного):<ul><li>если данные ТТН сходятся с количеством поставленного товара, то в ЕГАИС уходит подтверждение о получении товара;</li><li>если обнаружена недостача или излишек, то партию товара можно принять (целиком или частично), либо отказаться от нее - в этом случае в ЕГАИС также уходит подтверждение;</li></ul></li><li>объем товара, подтверждение о приемке которого передано в ЕГАИС, списывается с остатков производителя (оптового поставщика) и зачисляется на баланс покупателя (оптовика или магазина) в личных кабинетах ЕГАИС покупателя и поставщика;</li><li>при продаже маркированного алкоголя в розницу марки на бутылках сканируются и данные цифрового идентификатора марки отправляются в ЕГАИС, где сопоставляются и проверяются с данными ЕГАИС (при реализации алкоголя через точки общепита (в том числе в розлив), сканирование и передача данных в ЕГАИС происходят в момент открытия бутылки);</li><li>после проверки ЕГАИС присылает ответ о том, что цифровой идентификатор найден, либо отказ в регистрации марки. В случае отказа поставщик должен разобраться с происхождением бутылки, не прошедшей проверку. </li></ol><h3>Формат ЕГАИС 3.0 для оптовых продавцов алкогольной продукции</h3><p>С 01.07.2018 все участники алкогольного рынка переходят на формат ЕГАИС 3.0, предполагающий помарочный (побутылочный) учет маркируемой алкогольной продукции. Это означает, что при перемещении алкогольной продукции в систему ЕГАИС передается информация не только о партии перемещаемой продукции, но и о движении каждой включенной в эту партию бутылки. Наклеиваемые на бутылку марки теперь выдаются производителем алкоголя не на партию, а привязываются к каждой конкретной бутылке и позволяют отследить информацию о ее происхождении и перемещении. Это, в свою очередь, позволит избежать попадания в продажу контрафактной продукции. </p><p>Помарочный учет каждой бутылки обязателен также при перемещении алкогольной продукции между складами и магазинами. Несмотря на это участникам оптового алкогольного рынка нет необходимости сканировать каждую бутылку при перемещении партии алкоголя между складами или торговыми точками. Для этих целей в ЕГАИС предусмотрена возможность поставки, отгрузки и хранения алкоголя в маркированной групповой таре (например, коробах или паллетах). Вместо сканирования каждой содержащейся в такой таре бутылки, для отражения в ЕГАИС факта принятия товара достаточно отсканировать нанесенный на тару штрихкод. При этом информация о цифровых индикаторах алкогольной продукции, находящейся в этой таре, заполнится в системе автоматически. </p><p>Сведения об упаковке, включая нанесенный на нее штрихкод и иерархию вложений, формируются в системе лицом, производящим упаковку (например, производителем) и отражаются во входящей товарно-транспортной накладной. </p><h3>Подключение к ЕГАИС поставщиков алкогольной продукции</h3><p>Поставщики алкогольной и спиртосодержащей продукции подключаются к системе ЕГАИС самостоятельно с использованием Универсального транспортного модуля (УТМ).</p><p>Для подключения к системе необходимо следующее оборудование (информация представлена на <a href="http://wiki.egais.ru/wiki/Подключение_к_ЕГАИС_оптовиков,_осуществляющих_закупку,_хранение_и_поставку_алкогольной_и_спиртосодержащей_продукции" target="_blank">сайте ЕГАИС</a>):</p><ul><li>Компьютер (рабочая станция):<ul><li>процессор х32 с частотой от 1,9 ГГц и выше</li><li>оперативная память объемом от 2 Гб или более</li><li>дисковый накопитель объемом не менее 50 Гб</li><li>Ethernet контроллер, 100/1000 Mbps, разъем RJ45</li></ul></li><li>Криптографическое оборудование (аппаратный крипто-ключ JaCarta со встроенным криптопровайдером PKI/ГОСТ)</li><li>Операционная система Windows 7 Starter и выше</li><li>Общесистемное программное обеспечение Java 8 и выше</li><li>Программное обеспечение ЕГАИС – УТМ (скачивается самостоятельно на сайте Росалкогольрегулирования через <a href="https://service.egais.ru/checksystem" target="_blank">личный кабинет</a>)</li><li>Усиленная квалифицированная электронная подпись (для ее оформления необходима выписка из ЕГРЮЛ (ЕГРИП), СНИЛС, ИНН, ОГРН, Паспорт ИП).</li><li>Бухгалтерская программа с возможностью взаимодействия с УТМ и возможностью формирования файла установленного формата для отправки в ЕГАИС.</li></ul><p>Для взаимодействия с ЕГАИС необходимо стабильное интернет-соединение со скоростью не менее 256 кбит/с. Однако в случае сбоя соединения можно продолжать работу. Универсальный транспортный модуль может работать без подключения к Интернету до трех дней, накапливая информацию о реализованном алкоголе. Когда интернет-соединение будет восстановлено, информация поступит в ЕГАИС.</p><p>Чтобы подключиться к ЕГАИС, нужно выполнить следующие действия:</p><ol><li>Приобрести защищенный носитель JaCarta PKIГОСТSE и усиленную квалифицированную электронную подпись (КЭП), которая будет использоваться для входа в личный кабинет на портале ЕГАИС, а также для подписания электронных документов перед их фиксацией в ЕГАИС. О том, как их можно приобрести, читайте <A href="/db/content/egais/src/этп/1. покупка защищенного носителя jacarta и кэп.htm?_=1544082579">подробнее</a>. Для корректной работы с ЕГАИС и УТМ необходимо установить "Единый клиент JaCarta" (см. <A href="/db/content/egais/src/этп/2. установка _единого клиента jacarta_.htm?_=1544082579">подробнее</a>).</li><li>Получить в <a href="https://service.egais.ru/checksystem" target="_blank">личном кабинете</a> на сайте ФС РАР сертификат для установки защищенного соединения с ЕГАИС (RSA-ключ). Он также необходим для идентификации организации в системе. Подробный порядок действий приведен в статьях: <A href="/db/content/egais/src/этп/3. настройка рабочего места для входа в личный кабинет егаис.htm?_=1544082579">Настройка рабочего места для входа в личный кабинет ЕГАИС</a>, <A href="/db/content/egais/src/этп/4. настройка торговых точек в личном кабинете егаис.htm?_=1544082579">Настройка торговых точек в личном кабинете ЕГАИС</a>, <A href="/db/content/egais/src/этп/5. формирование rsa – ключей для каждой торговой точки.htm?_=1544082579">Формирование RSA-ключей для каждой торговой точки</a>.</li><li>Скачать в <a href="https://service.egais.ru/login" target="_blank">личном кабинете</a> на сайте ФС РАР бесплатный Универсальный транспортный модуль (УТМ). Порядок получения дистрибутива УТМ и его установки рассмотрен в статье <A href="/db/content/egais/src/этп/6. универсальный транспортный модуль (утм).htm?_=1544082579">"Универсальный транспортный модуль"</a>.</li><li>Настроить учетную программу на взаимодействие с ЕГАИС. О настройке программ 1С и работе с ЕГАИС читайте в рубрике <A href="/db/content/egais/src/1с егаис.htm?_=1544082579">"Организация продаж алкогольной продукции в программах 1С"</a>.</li></ol> ',example2:'<body><p>На <a href="http://v8.1c.ru/libraries/cel/certified.htm" target="_blank">сайте</a> обновлена страница с оборудованием – выделены модели фискальных регистраторов с поддержкой ЕГАИС.</body>',example3:'<body lang="RU" class="paywall"><div class="header">Учет по налогу на прибыль в государственных учреждениях</div><div id=actinfo><p>Дата публикации: 10.07.2014</p><p>1С:БГУ 8 ред. 2, релиз 2.0.25</p></div><p>&nbsp;</p><h2>Регламентные операции налогового учета</h2><p><aname="_toc324599979_1"></a>&nbsp;Для ввода регламентных операций рекомендуется использовать<b>&quot;Помощникзакрытия периода&quot;</b> (раздел &quot;Учет и отчетность - Регламентныеоперации&quot;).</p><h3><b>Закрытиепроизводственных счетов</b></h3><p>Расчет,распределение и списание расходов на производство и реализацию производятсярегламентным документом <b>&quot;Закрытие производственных счетов&quot;</b>(раздел <b>&quot;Услуги, работы, производство&quot;</b>, группа команд панелинавигации <b>&quot;Затраты на производство&quot;</b>).</p><blockquote><p><IMG src="_images/image015.gif?_=1544610819" WIDTH="842" ALT="i8105583.filesImage2587.gif" HEIGHT="550" BORDER="0"></p></blockquote><p>Документ <b>&quot;Закрытиепроизводственных счетов&quot;</b> в части закрытия счетов налогового учетапроизводит:</p><ul style="margin-top:0cm" type=disc><li class=MsoNormal style="margin-bottom:10.0pt">расчет доли особых режимов налогообложения в общейдеятельности (доля ЕНВД); </li><li class=MsoNormal style="margin-bottom:10.0pt">списание косвенных расходов производства и реализациис учетом норм (для нормируемых расходов) и доли ЕНВД (для распределяемыхрасходов) на расходы текущего периода; </li><li class=MsoNormal style="margin-bottom:10.0pt">распределение общих затрат на стоимость продукции,работ, услуг согласно установленной в учетной политике базе распределения;</li><li class=MsoNormal style="margin-bottom:10.0pt">расчет фактической стоимости продукции; </li><li class=MsoNormal style="margin-bottom:10.0pt">корректировку оборотов, выполненных в течение месяцапо плановым ценам выпущенной продукции, до оборотов по фактическойстоимости. </li></ul><p><b>Примечание. </b>Для распределения общепроизводственных и общехозяйственныхзатрат в налоговом учете применяется тот же порядок, что и в бухгалтерскомучете.</p><p>Порядок распределения общепроизводственных иобщехозяйственных затрат устанавливается в учетной политике организации назакладке <b>&quot;Производство&quot;</b> с помощью флажков <b>&quot;Распределятьобщепроизводственные затраты&quot;</b>, <b>&quot;Распределять общехозяйственныезатраты&quot;</b> и методов их распределения, указанных в форме <b>&quot;Методыраспределения косвенных расходов&quot;</b>.</p><blockquote><p><IMG src="_images/image006.gif?_=1544610819" WIDTH="894" ALT="i8105583.filesImage2588.gif" HEIGHT="638" BORDER="0"></p></blockquote><h3><aname="_toc324599979_2"></a><b>Списание затрат по услугам</b></h3><p>Длясписания расходов по услугам и работам, аккумулированных на счетах налоговогоучета производственных расходов и издержек обращения Н20, Н25, Н26 и Н44,применяется регламентный документ <b>&quot;Списание затрат по услугам&quot; </b>(раздел<b>&quot;Услуги, работы, производство&quot;</b>).</p><p>Информацияо расходах налогового учета указывается в сводной таблице затрат в реквизитах <b>&quot;Сумма(НУ)&quot;</b>, <b>&quot;Не учитывается (НУ)&quot;</b>, <b>&quot;ЕНВД(НУ)&quot;</b> и в детальной таблице на закладке <b>&quot;Налоговый учет&quot;</b>.</p><blockquote><p><IMG src="_images/image016.gif?_=1544610819" WIDTH="803" ALT="i8105583.filesImage2589.gif" HEIGHT="765" BORDER="0"></p></blockquote><h3><aname="_toc324599979_3"></a><b>Расчет налога на прибыль</b></h3><p>Расчетналога на прибыль в программе выполняется в два этапа:</p><h4><b>Расчет налоговойбазы по налогу на прибыль </b></h4><p>Расчет налоговой базы выполняется регламентной операцией <b>&quot;Расчетбазы по налогу на прибыль&quot;</b> (раздел <b>&quot;Учет и отчетность&quot;</b>,группа команд панели навигации <b>&quot;Регламентные операции&quot;</b>).</p><p>При проведении документа <b>&quot;Расчет базы по налогу наприбыль&quot;</b> формируется налоговая база на счете Н99 &quot;Прибыли иубытки&quot; в корреспонденции со счетами Н90.09 &quot;Прибыль / убыток отпродаж&quot; и Н91.09 &quot;Сальдо прочих доходов и расходов&quot;.</p><h4><b>Расчет налога наприбыль </b></h4><p>Расчетналога на прибыль к уплате и его отражение в бухгалтерском учете выполняютсярегламентной операцией <b>&quot;Расчет налога на прибыль&quot;</b> (раздел <b>&quot;Учети отчетность&quot;</b>, группа команд панели навигации <b>&quot;Регламентныеоперации&quot;</b>). </p><p>Документ <b>&quot;Расчетналога на прибыль&quot;</b> рассчитывает сумму налога на прибыль к уплате вфедеральный бюджет и в бюджет субъекта РФ. Рассчитанные суммы налога на прибыльмогут быть распределены по аналитике счета 401.10 &quot;Доходы текущегофинансового года&quot; (КПС, КЭК, направлениям деятельности и пр.) исходя изприходящейся на них доли прибыли. При проведении документа формируютсябухгалтерские записи по начислению налога на прибыль Д-т 401.10, К-т 303.03. </p><p>Документы <b>&quot;Расчет базы по налогу на прибыль&quot;</b>и <b>&quot;Расчет налога на прибыль&quot;</b> следует формировать в программеежемесячно.</p><blockquote><p><IMG src="_images/image017.gif?_=1544610819" WIDTH="834" ALT="i8105583.filesImage2590.gif" HEIGHT="791" BORDER="0"></p></blockquote><p><b>Примечание</b>. Ставки налога на прибыль устанавливаются по гиперссылке <b><u>Налоговыеставки</u></b> в форме документа <b>&quot;Расчет налога на прибыль&quot;</b>.</p><p>&nbsp;</p><p>&nbsp;</p></div></body>'}),methods:{getArticle:async function(t,e){let s={method:"get",url:"http://www.u0612907.plsk.regruhosting.ru/api/Article/GetArticle",headers:{Authorization:"Bearer "+this.token},params:{id:t,query:e}},i=await r()(s);return console.log(i),i.data}},mounted(){(async()=>{})()}},w={render:function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("div",[s("v-container",[""==t.article?s("div",{staticClass:"article",domProps:{innerHTML:t._s(t.example3)}}):t._e(),t._v(" "),""!=t.article?s("div",{staticClass:"article",domProps:{innerHTML:t._s(t.article.title+"\n"+t.article.text)}}):t._e()]),t._v(" "),s("modal-component",{attrs:{token:t.token,sessionId:t.sessionId}})],1)},staticRenderFns:[]};var y=s("VU/8")(k,w,!1,function(t){s("7ZZq")},"data-v-78ccaed8",null).exports;i.default.use(d.a);var H=new d.a({routes:[{path:"/",name:"MainView",component:x},{path:"article/:articleId:token",component:y,name:"ArticlePage",props:!0}]}),R=s("3EgV"),I=s.n(R);i.default.use(I.a),i.default.config.productionTip=!1,new i.default({el:"#app",router:H,components:{App:h},template:"<App/>",methods:{}})},qU6w:function(t,e){},qrsy:function(t,e){}},["NHnr"]);
//# sourceMappingURL=app.5f530dfe8c94d808e293.js.map