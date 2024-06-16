addJsEnd('/plugin/jquery-validation/jquery.validate.min.js', true);

addLoadEvent(function () {

    var jvalidate = $("#page-group-form").validate({
        //debug: true,
        rules: {
            PageGroupName: {
                required: true
            }
        },
        messages: {
            PageGroupName: {
                required: __val_pageGroupName
            }
        },

    });

    $('#reset-form').click(function () {
        resetForm($("#page-group-form"));

        $('#page-group-msg').text('');
        $('#page-group-msg').hide();
    });

    $('#back-pg-btn').click(function () {
        window.location.replace('/Admin/'+__langMark+'/PageGroups/');
    });


    //999/ تابعی برای انتخاب کردن دراپ.دان اصلی و مخفی 
    // سلکت اچ.تی.ام.الی مخفی باید انتهای نامش 
    //_select
    //را داشته باشد
    $('.dropdown ul li').each(function() {
        $(this).click(function(){
            var val = $(this).attr('data-id');
            var id = $(this).parent().parent().attr('id')+'_select';
            $('#'+id+' option').each(function(){
                $(this).removeAttr('selected');     
            });
            $('#'+id+' option[value="'+val+'"]').attr('selected', 'selected');

            $(this).parent().find('li').removeClass('active');
            $(this).addClass('active');
            var btn = $(this).parent().parent().find('button');
            btn.html(__lang+' : '+$(this).text()+' <span class="caret"></span>');
        });
    });
});

function onBeginCreate(){
    $('#create-pg-btn').attr({ 'disabled': 'disabled', 'value': __submiting });

    $('#dialogWait').modal({ keyboard: false, backdrop: 'static' });
    $('#page-group-msg').hide();
}

function onFailedCreate(data){
    $('#dialogWait').modal('hide');
    $('#page-group-msg').text("Failed /n"+data);
    $('#page-group-msg').show("slow");
    $('#create-pg-btn').removeAttr("disabled").attr({'value': __create });
}

function onSuccessCreate(data){
    if (data.res.toString().toLowerCase().trim() == "ok") {
        alert(data.msg);
        window.location.replace('/admin/'+__langMark+'/PageGroups/');
        return;
    } else {
        $('#create-pg-btn').removeAttr("disabled").attr({'value': __create });

        $('#page-group-msg').text(data.msg);
        $('#page-group-msg').show("slow");
    }
    $('#dialogWait').modal('hide');
}
