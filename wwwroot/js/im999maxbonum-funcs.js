function resetForm($form) {
    //$form.find('input[type=text], input[type=password], input[type=email], input[type=file], select, textarea').val('');
    $form.find('input[type=text], input[type=password], input[type=email], input[type=file], textarea').val('');
    $form.find('input[type=radio], input[type=checkbox]').removeAttr('checked').removeAttr('selected');
    //$form.find('input:text, input:password, input:email, input:file, select, textarea').val('');
    //$form.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');


    $form.validate().resetForm();
    //$form.find("[data-valmsg-replace]").empty();
    //$form.find("[data-valmsg-replace]").removeClass("field-validation-error");

    //$form.find("[data-valmsg-replace]").removeClass("input-validation-error");//.addCss("input-validation-valid");
}

//add js func to window.onload Event
function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function () {
            if (oldonload) {
                oldonload();
            }
            func();
        }
    }
}

//add css to header 
function addCss(path, last) {
    if ($("head link[href='" + path + "']").length > 0) return;

    if (last)
        $("head link[rel='stylesheet']").last().after("<link rel='stylesheet' href='" + path + "' type='text/css' />");
    else
        $("head link[rel='stylesheet']").first().before("<link rel='stylesheet' href='" + path + "' type='text/css' />");
}

//add js to header 
function addJs(path, last) {
    if ($("head script[src='" + path + "']").length > 0) return;

    if (last)
        $("head script[type='text/javascript']").last().after("<script type='text/javascript' src='" + path + "' ></script>");
    else
        $("head script[type='text/javascript']").first().before("<script type='text/javascript' src='" + path + "' ></script>");
}

//add js to End 
function addJsEnd(path, last) {
    if ($("#addin-end").children("script[src='" + path + "']").length > 0)
        return;

    if (last)
        $("#addin-end").children().last().after("<script type='text/javascript' src='" + path + "' ></script>");
    else
        $("#addin-end").children().first().before("<script type='text/javascript' src='" + path + "' ></script>");
}

function setCookie(cname, cvalue, exmin) {
    var d = new Date();
    d.setTime(d.getTime() + (exmin*60*1000));//*(exdays*24*60*60*1000));
    var expires = "expires="+ d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for(var i = 0; i <ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function GetIraniString(strCode)
{
    var str = strCode;
    if(str == null)
        return null;

    str = str.split("&#x60C;").join("،");    
    str = str.split("&#x61B;").join("؛");    
    str = str.split("&#x61F;").join("؟");    
    str = str.split("&#x621;").join("ء");    
    str = str.split("&#x622;").join("آ");    
    str = str.split("&#x623;").join("أ");    
    str = str.split("&#x624;").join("ؤ");    
    str = str.split("&#x625;").join("إ");    
    str = str.split("&#x626;").join("ئ");    
    str = str.split("&#x627;").join("ا");    
    str = str.split("&#x628;").join("ب");
    str = str.split("&#x629;").join("ة");
    str = str.split("&#x62A;").join("ت");
    str = str.split("&#x62B;").join("ث");
    str = str.split("&#x62C;").join("ج");
    str = str.split("&#x62D;").join("ح");
    str = str.split("&#x62E;").join("خ");
    str = str.split("&#x62F;").join("د");
    str = str.split("&#x630;").join("ذ");
    str = str.split("&#x631;").join("ر");
    str = str.split("&#x632;").join("ز");
    str = str.split("&#x633;").join("س");
    str = str.split("&#x634;").join("ش");
    str = str.split("&#x635;").join("ص");
    str = str.split("&#x636;").join("ض");
    str = str.split("&#x637;").join("ط");
    str = str.split("&#x638;").join("ظ");
    str = str.split("&#x639;").join("ع");
    str = str.split("&#x63A;").join("غ");
    str = str.split("&#x640;").join("ـ");
    str = str.split("&#x641;").join("ف");
    str = str.split("&#x642;").join("ق");
    str = str.split("&#x644;").join("ل");
    str = str.split("&#x645;").join("م");
    str = str.split("&#x646;").join("ن");
    str = str.split("&#x647;").join("ه");
    str = str.split("&#x648;").join("و");
    str = str.split("&#x64A;").join("ي");
    str = str.split("&#x64B;").join("ً");
    str = str.split("&#x64C;").join("ٌ");
    str = str.split("&#x64D;").join("ٍ");
    str = str.split("&#x64E;").join("َ");
    str = str.split("&#x64F;").join("ُ");
    str = str.split("&#x650;").join("ِ");
    str = str.split("&#x651;").join("ّ");
    str = str.split("&#x67E;").join("پ");
    str = str.split("&#x686;").join("چ");
    str = str.split("&#x698;").join("ژ");
    str = str.split("&#x6A9;").join("ک");
    str = str.split("&#x6AF;").join("گ");
    str = str.split("&#x6C0;").join("ۀ");
    str = str.split("&#x6CC;").join("ی");
    return str;
}
