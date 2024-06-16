//addLoadEvent(function () {

    addJsEnd('/plugin/jquery-validation/jquery.validate.min.js', true);
    //addJsEnd('/plugin/jquery-validation/jquery.validate.min.0.16.test.js', true);
//});

/*
jQuery.validator.setDefaults({
    submitHandler: function() {
        alert("submitted!");
    }
});
*/

addLoadEvent(function () {
    //LoginClearAll();

    $('#retry_captcha_1').click(function () { 
        reLoadCaptcha('CaptchaImage1', $('#CaptchaImage1').data("prefix")); return false; 
    });

    var jvalidate = $("#login-form").validate({
        //debug: true,
        rules: {
            Email: {
                email: true
            },
            UserName: "check_val",
            Password: {
                required: true
            },
            captcha: {
                required: true
            }
        },
        messages: {
            Email: {
                email: __val_email
            },
            Password: {
                required: __val_password
            },
            captcha: {
                required: __val_captcha
            }
        },

    });

    jQuery.validator.addMethod("check_val", function(value) {
        var ret = false;
        if($("#Email").val())
            ret = true;
        if($("#UserName").val())
            ret = true;
        return ret; }, 
        __val_username
    );
    
    $('#reset-form').click(function () {
        //jvalidate.resetForm();
        //$("#login-form").validate().resetForm()
        resetForm($("#login-form"));

        $('#login-msg').text('');
        $('#login-msg').hide();
    });

    $('#back-homepage').click(function () {
        window.location.replace('/'+__langMark+'/');
    });
});

function reLoadCaptcha(imgId, perfix) {
    var myImageElement = document.getElementById(imgId);// + perfix.toString());
    //myImageElement.src = '/General/CaptchaImage?prefix=' + perfix + '&random=' + Math.random();
    myImageElement.src = '/General/CaptchaImage?guids='+__guids+'&prefix=' + perfix + '&random=' + Math.random();
}

var onFailedLogin = function(data){
    //alert("Failed");
    $('#dialogWait').modal('hide');
    $('#login-msg').text("Failed /n"+data);
    $('#login-msg').show("slow");
    reLoadCaptcha('CaptchaImage1', $('#CaptchaImage1').data("prefix"));
    $('#form-login-btn').removeAttr("disabled").attr({'value': __login });
};

var onSuccessLogin = function(data){
    //alert('success');
    if (data.res.toString().toLowerCase().trim() == "ok") {
        alert(__welcome);
        //window.location = event.msg;
        location.reload(true);
        //window.location.href = '/';
        //window.location.replace('/'+__langMark+'/');
        return;
    } else {
        //reLoadCaptcha("CaptchaImage", 1);        
        //document.getElementById(retry_captcha_1).click();
        reLoadCaptcha('CaptchaImage1', $('#CaptchaImage1').data("prefix"));
        $('#form-login-btn').removeAttr("disabled").attr({'value': __login });

        $('#login-msg').text(data.msg);
        $('#login-msg').show("slow");
    }
    //$('#dialogWait').modal('toggle');
    //$('#dialogWait').modal().hide();
    $('#dialogWait').modal('hide');
};

//var onBeginLogin = function(){
function onBeginLogin(){
    //alert('begin');
    $('#form-login-btn').attr({ 'disabled': 'disabled', 'value': __submiting });

    //results.html("<img src=\"/images/ajax-loader.gif\" alt=\"Loading\" />");
    $('#dialogWait').modal({ keyboard: false, backdrop: 'static' });
    $('#login-msg').hide();
}

/*
function LoginClearAll() {
    $('#login-msg-main').text("");
    $('#login-msg-main').hide();
    resetForm($('#login-form'));

    alert($('input, textarea'));
    $('input, textarea').placeholder({ customClass: 'my-placeholder' });

    reLoadCaptcha("CaptchaImage", 1);
}
*/