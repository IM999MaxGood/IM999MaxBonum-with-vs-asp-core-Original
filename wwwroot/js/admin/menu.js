//بخشی برای باز کردن زیر منوها در حالتی که منو موبایلی است
var $isSetMenu = false;
function setMenu($ul,$a){
	if ($ul.is(':visible')) {
		$ul.css({display: ''});
		$a.attr('class','');
	} else {
		$ul.css({display: 'block'});
		$a.attr('class','mines');
	}							

}

//متد آن کلیک دکمه نمایش منو
var $isMenuClose= true;
function closeMenuMobile(doAnime){
	if(doAnime)
		$('.mainmenu').animate({"right":"-300px"}, "slow");
	else
		$('.mainmenu').css({"right":"-300px"});		
	$('.mainmenu ul li ul').each(function( ) {	
		$(this).css({display: ''});	
		$(this).parent().find('a:first').attr('class','');	
	});
	$('.mainmenu').css({"display":"none"});
	$('.mainmenu').insertAfter('#menuToggle');
	$('#menuToggle').find('i').removeClass('fa-remove').addClass('fa-bars');
	$isMenuClose= true;
}

function openMenuMobile(){
	$('#menuToggle').find('i').removeClass('fa-bars').addClass('fa-remove');
	$('.mainmenu').prependTo("body");
	$('.mainmenu').animate({"right":"0px"}, "slow");
	$('.mainmenu').css({"display":"block"});
	$isMenuClose= false;
}
 
$('#menuToggle').click(function(){
	if($isMenuClose)
		openMenuMobile();
	else
		closeMenuMobile(true);
}); 

$('#close-menu').click(function(){
	if($isMenuClose)
		openMenuMobile();
	else
		closeMenuMobile(true);
}); 

/*این بخش برای ست کردن آن کلیک برای نماش زیر منوها*/
$('.mainmenu ul li ul').each(function( ) {
    var $ulP = $(this);
    $(this).parent().find('a:first').click(function(){
        setMenu($ulP, $(this));
    });
});

//متد تغییر سایز ویندوز
function changeWinSize(){
	if($(window).width()<949){
		closeMenuMobile();
		
		if($isSetMenu)
			return;
		$('.mainmenu').css({'display': 'none'});
		$('#menuToggle').css({'display': 'block'});
/*
        $('.mainmenu ul li ul').each(function( ) {
			var $ulP = $(this);
			$(this).parent().find('a:first').click(function(){
				setMenu($ulP, $(this));
			});
		});
*/
		$isSetMenu=true;
	}else{
		if(!$isSetMenu)
			return;
/*
        $('.mainmenu ul li ul').each(function( ) {
			var $ulP = $(this);
			$(this).parent().find('a:first').click(function(){
				setMenu($ulP, $(this));
			});
		});
*/
		closeMenuMobile(false);
		$('.mainmenu').css({'left':'','right':''});
		$('.mainmenu').css({'display': ''});
		$('#menuToggle').css({'display': ''});
		$isSetMenu=false;
	}
	 
} 
 
$(window).on("resize", function(){
	changeWinSize();
});

$(window).on("load", function(){
	changeWinSize();
});

$( window ).on("orientationchange", function() {
	changeWinSize();
});

