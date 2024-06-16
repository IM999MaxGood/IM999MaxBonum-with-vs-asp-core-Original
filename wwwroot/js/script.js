function GetHeightSlider(newWidth){//orginalWidth,originalHeight){
	var aspectRatio = 550/1366;//768/1366;//originalHeight / orginalWidth;
	return newWidth * aspectRatio;
}

function resizeHeight(){
	$('.main-slider-container').css('cssText', 'height: '+GetHeightSlider($(window).width()).toString()+'px !important;');
}

	
function loadSlider() {

	var jssor_1_SlideshowTransitions = [
		{$Duration:800,$Opacity:2}
	];

	var jssor_1_SlideoTransitions = [
		[{b:2800,d:600,y:70,sX:-0.5,sY:-0.5,e:{y:5}},{b:6000,d:600,y:50,r:-10},{b:7000,d:400,o:-1,rX:10,rY:-10}],
		[{b:0,d:600,x:-742,sX:4,sY:4,e:{x:6}},{b:900,d:600,sX:-4,sY:-4}],
		[{b:-1,d:1,o:-1},{b:400,d:500,o:1,e:{o:5}}],
		[{b:-1,d:1,o:-1,r:-180},{b:1500,d:500,o:1,r:180,e:{r:27}}],
		[{b:-1,d:1,o:-1,r:180},{b:2000,d:500,o:1,r:-180,e:{r:27}}],
		[{b:2800,d:600,y:-270,e:{y:6}}],
		[{b:6000,d:600,y:-100,r:-10,e:{y:6}},{b:7000,d:400,o:-1,rX:-10,rY:10}],
		[{b:-1,d:1,sX:-1,sY:-1},{b:3400,d:400,sX:1.33,sY:1.33,e:{sX:7,sY:7}},{b:3800,d:200,sX:-0.33,sY:-0.33,e:{sX:16,sY:16}}],
		[{b:-1,d:1,o:-1},{b:3400,d:600,o:1},{b:4000,d:1000,r:360,e:{r:1}}],
		[{b:-1,d:1,o:-1},{b:3400,d:600,y:-70,o:1,e:{y:27}}],
		[{b:-1,d:1,sX:-1,sY:-1},{b:3700,d:400,sX:1.33,sY:1.33,e:{sX:7,sY:7}},{b:4100,d:200,sX:-0.33,sY:-0.33,e:{sX:16,sY:16}}],
		[{b:-1,d:1,o:-1},{b:3700,d:600,o:1},{b:4300,d:1000,r:360}],
		[{b:-1,d:1,o:-1},{b:3700,d:600,x:-150,o:1,e:{x:27}}],
		[{b:-1,d:1,sX:-1,sY:-1},{b:4000,d:400,sX:1.33,sY:1.33,e:{sX:7,sY:7}},{b:4400,d:200,sX:-0.33,sY:-0.33,e:{sX:16,sY:16}}],
		[{b:-1,d:1,o:-1},{b:4000,d:600,o:1},{b:4600,d:1000,r:360}],
		[{b:-1,d:1,o:-1},{b:4000,d:600,x:150,o:1,e:{x:27}}],
		[{b:9300,d:600,o:-1,r:540,sX:-0.5,sY:-0.5,e:{r:5}}],
		[{b:-1,d:1,o:-1,sX:2,sY:2},{b:6880,d:20,o:1},{b:6900,d:300,sX:-2.08,sY:-2.08,e:{sX:27,sY:27}},{b:7200,d:240,sX:0.08,sY:0.08}],
		[{b:-1,d:1,o:-1,sX:5,sY:5},{b:6300,d:600,o:1,sX:-5,sY:-5}],
		[{b:-1,d:1,o:-1},{b:7200,d:440,o:1}],
		[{b:-1,d:1,o:-1,sX:-0.9,sY:-0.9},{b:7420,d:20,o:1},{b:7440,d:200,r:180,sX:0.4,sY:0.4},{b:7640,d:200,r:180,sX:0.5,sY:0.5}],
		[{b:-1,d:1,o:-1,r:-60,sX:-0.9,sY:-0.9},{b:7620,d:20,o:1},{b:7640,d:300,r:60,sX:1.1,sY:1.1},{b:7940,d:160,sX:-0.2,sY:-0.2}],
		[{b:-1,d:1,o:-1,sX:-0.9,sY:-0.9},{b:7920,d:20,o:1},{b:7940,d:300,sX:1.4,sY:1.4},{b:8240,d:160,sX:-0.5,sY:-0.5}],
		[{b:-1,d:1,o:-1,sX:-0.9,sY:-0.9},{b:7620,d:20,o:1},{b:7640,d:200,r:180,sX:0.4,sY:0.4},{b:7840,d:200,r:180,sX:0.5,sY:0.5}],
		[{b:-1,d:1,o:-1,r:-60,sX:-0.9,sY:-0.9},{b:7820,d:20,o:1},{b:7840,d:300,r:60,sX:1.1,sY:1.1},{b:8140,d:160,sX:-0.2,sY:-0.2}],
		[{b:-1,d:1,o:-1,sX:-0.9,sY:-0.9},{b:8120,d:20,o:1},{b:8140,d:300,sX:1.4,sY:1.4},{b:8440,d:160,sX:-0.5,sY:-0.5}],
		[{b:-1,d:1,o:-1,sX:-0.9,sY:-0.9},{b:7820,d:20,o:1},{b:7840,d:200,r:180,sX:0.4,sY:0.4},{b:8040,d:200,r:180,sX:0.5,sY:0.5}],
		[{b:-1,d:1,o:-1,r:-60,sX:-0.9,sY:-0.9},{b:8020,d:20,o:1},{b:8040,d:300,r:60,sX:1.1,sY:1.1},{b:8340,d:160,sX:-0.2,sY:-0.2}],
		[{b:-1,d:1,o:-1,sX:-0.9,sY:-0.9},{b:8320,d:20,o:1},{b:8340,d:300,sX:1.4,sY:1.4},{b:8640,d:160,sX:-0.5,sY:-0.5}],
		[{b:-1,d:1,o:-1,sX:-0.9,sY:-0.9},{b:8020,d:20,o:1},{b:8040,d:200,r:180,sX:0.4,sY:0.4},{b:8240,d:200,r:180,sX:0.5,sY:0.5}],
		[{b:-1,d:1,o:-1,r:-60,sX:-0.9,sY:-0.9},{b:8220,d:20,o:1},{b:8240,d:300,r:60,sX:1.1,sY:1.1},{b:8540,d:160,sX:-0.2,sY:-0.2}],
		[{b:-1,d:1,o:-1,sX:-0.9,sY:-0.9},{b:8520,d:20,o:1},{b:8540,d:300,sX:1.4,sY:1.4},{b:8840,d:160,sX:-0.5,sY:-0.5}],
		[{b:-1,d:1,o:-1,sX:-0.9,sY:-0.9},{b:8220,d:20,o:1},{b:8240,d:200,r:180,sX:0.4,sY:0.4},{b:8440,d:200,r:180,sX:0.5,sY:0.5}],
		[{b:-1,d:1,o:-1,r:-60,sX:-0.9,sY:-0.9},{b:8420,d:20,o:1},{b:8440,d:300,r:60,sX:1.1,sY:1.1},{b:8740,d:160,sX:-0.2,sY:-0.2}],
		[{b:-1,d:1,o:-1,sX:-0.9,sY:-0.9},{b:8720,d:20,o:1},{b:8740,d:300,sX:1.4,sY:1.4},{b:9040,d:160,sX:-0.5,sY:-0.5}],
		[{b:-1,d:1,o:-1,sX:-0.9,sY:-0.9},{b:8220,d:20,o:1},{b:8240,d:200,r:180,sX:0.4,sY:0.4},{b:8440,d:200,r:180,sX:0.5,sY:0.5}],
		[{b:-1,d:1,o:-1,r:-60,sX:-0.9,sY:-0.9},{b:8420,d:20,o:1},{b:8440,d:300,r:60,sX:1.1,sY:1.1},{b:8740,d:160,sX:-0.2,sY:-0.2}],
		[{b:-1,d:1,o:-1,sX:-0.9,sY:-0.9},{b:8720,d:20,o:1},{b:8740,d:300,sX:1.4,sY:1.4},{b:9040,d:160,sX:-0.5,sY:-0.5}],
		[{b:-1,d:1,o:-1},{b:0,d:1200,y:300,o:1,e:{y:24,o:6}},{b:5600,d:800,y:-200,o:-1,e:{y:5}}],
		[{b:-1,d:1,o:-1},{b:400,d:800,x:200,o:1,e:{x:27,o:6}},{b:5600,d:800,x:-200,o:-1,e:{x:5}}],
		[{b:-1,d:1,o:-1},{b:400,d:800,x:-200,o:1,e:{x:27,o:6}},{b:5600,d:800,x:200,o:-1,e:{x:5}}],
		[{b:-1,d:1,o:-1},{b:1600,d:600,x:-151,y:-200,o:1,e:{x:3,y:3}},{b:5600,d:800,o:-1}],
		[{b:4600,d:960,x:-204,e:{x:1}}],
		[{b:-1,d:1,sX:-1,sY:-1},{b:3400,d:400,sX:1,sY:1},{b:3800,d:300,o:-1,sX:0.1,sY:0.1}],
		[{b:-1,d:1,sX:-1,sY:-1},{b:3520,d:400,sX:1,sY:1},{b:3920,d:300,o:-1,sX:0.1,sY:0.1}],
		[{b:-1,d:1,o:-1},{b:2200,d:1200,x:-135,y:-24,o:1,e:{x:7,y:7}},{b:4600,d:640,x:-130,e:{x:1}}],
		[{b:-1,d:1,o:-1},{b:4600,d:240,x:-75,o:1},{b:4840,d:480,x:-150},{b:5320,d:240,x:-75,o:-1}],
		[{b:0,d:1160,x:783,e:{x:6}}],
		[{b:1160,d:840,x:792,y:-28,e:{x:12,y:3}}],
		[{b:2780,d:520,x:-510,y:-17,e:{x:6}},{b:4000,d:600,x:506,y:10,e:{x:5}}],
		[{b:3300,d:640,x:-23,y:-174,e:{y:6}},{b:4000,d:600,x:2,y:155,e:{y:5}}],
		[{b:2020,d:760,y:-397,e:{y:6}},{b:4000,d:600,x:-334,e:{x:5}}]
	];

	var jssor_1_options = {
		$AutoPlay: 1,
		$PauseOnHover: 0,
		$SlideshowOptions: {
			$Class: $JssorSlideshowRunner$,
			$Transitions: jssor_1_SlideshowTransitions
		},
		$CaptionSliderOptions: {
			$Class: $JssorCaptionSlideo$,
			$Transitions: jssor_1_SlideoTransitions,
			$Breaks: [
				[{d:3000,b:9300}],
				[{d:3000,b:5600}],
				[{d:3000,b:4000},{d:3000,b:4000}]
			]
		},
		$ArrowNavigatorOptions: {
			$Class: $JssorArrowNavigator$
		},
		$BulletNavigatorOptions: {
			$Class: $JssorBulletNavigator$
		}
	};

	var jssor_1_slider = new $JssorSlider$("main-slider-container", jssor_1_options);

	/*#region responsive code begin*/

	var MAX_WIDTH = 1366;

	function ScaleSlider() {
		var containerElement = jssor_1_slider.$Elmt.parentNode;
		var containerWidth = containerElement.clientWidth;

		if (containerWidth) {

			var expectedWidth = Math.min(MAX_WIDTH || containerWidth, containerWidth);

			jssor_1_slider.$ScaleWidth(expectedWidth);
		}
		else {
			window.setTimeout(ScaleSlider, 30);
		}
	}

	ScaleSlider();

	$Jssor$.$AddEvent(window, "load", ScaleSlider);
	$Jssor$.$AddEvent(window, "resize", ScaleSlider);
	$Jssor$.$AddEvent(window, "orientationchange", ScaleSlider);
	/*#endregion responsive code end*/
};



function fixTopMenu(){
	var menubarTop = $('#main-menu-bar').offset().top;

	$(window).scroll(function(){
        if( $(window).scrollTop() > menubarTop ) {
			$('#main-menu-bar').attr('class', 'menu-bar-fix');
			$('#logo-tag').attr('class', 'logo-tag-fix');
			
        } else {
			$('#main-menu-bar').attr('class', 'menu-bar-rel');
			$('#logo-tag').attr('class', 'logo-tag-rel');
			
		}
    });
}

function init_map(titr2, address_Text, google_Map_Add) { 
	var myOptions = { zoom: 17, center: new google.maps.LatLng(36.3090327, 59.5035622), mapTypeId: google.maps.MapTypeId.ROADMAP }; 
	map = new google.maps.Map(document.getElementById("google-map"), myOptions); 
	marker = new google.maps.Marker({ map: map, position: new google.maps.LatLng(36.3090327, 59.5035622) }); 
	infowindow = new google.maps.InfoWindow({ content: "<br/><p style='color: black; direction: rtl; font-size: 14px;font-family: tahoma;'><span style='font-size: 18px; font-weight: 800; color: #93120c;'>"+titr2+"</span><br/> "+address_Text+"<br/> "+google_Map_Add+"</p>" }); 
	google.maps.event.addListener(marker, "click", function () { infowindow.open(map, marker); }); 
	infowindow.open(map, marker);
} 


$(function(){	
    //Initiat WOW JS
    new WOW().init();

	if(document.getElementById('container-slider') !== null){
		loadSlider();
	
		fixTopMenu();
	}
	
	//999/this code in https has error
	//if (typeof address_Text !== 'undefined')
	if(document.getElementById('google-map') !== null)
		try{
			google.maps.event.addDomListener(window, 'load', init_map(titr2, address_Text, google_Map_Add));
		}catch{}


	if(document.getElementById('instaphotos') !== null){
		var feed = new Instafeed({
			get: 'user',
			//userId: 1362427010,
			//accessToken: '1362427010.1677ed0.52280c6350554afb842a16bdac34c974',
			userId: 4657402663,
			accessToken: '4657402663.1677ed0.18ef5b40e1b7477eaaec5191b02e4353',
			target: 'instaphotos',
			resolution: 'standard_resolution',
			after: function() {
				
				var $a = $('#instaphotos').find('a');
				$a.attr('target', '_blank');
				$a.each(function(){
					var $temp = $(this).html();
					$(this).empty();
					$(this).append('<div class="grid"><figure class="effect-duke">'+$temp+'<figcaption><div class="icon-holder"><ul><li><a><i class="fa fa-info"></i></a></li><li><a><i class="fa fa-link"></i></a></li><li><a><i class="fa fa-heart"></i></a></li></ul></div></figcaption></figure></div>');
				});

				//اسکرول دار کردن پنل عکس بعد از لود کامل
				$("#instaphotos").mCustomScrollbar({
					autoHideScrollbar:true,
					theme:"rounded"
				});

			}
		});
		
		feed.run();
	}
	
    $('.backtop').click(function (event) {
        event.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, 1000);
        return false;
    })

	//////// -- loader -- /////////
	$(window).on("load",function(){	
		setTimeout(function(){ $('.loader').fadeOut(); }, 1000);
	});
	
	
	///main menu anchor
	$('a[href*=\\#]').on('click', function(event){     
		event.preventDefault();
		$('html,body').animate({scrollTop:$(this.hash).offset().top}, 500);
	});
	
});

