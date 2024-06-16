addJsEnd('/plugin/jquery-validation/jquery.validate.min.js', true);

addJsEnd('/plugin/tinymce/tinymce.min.js', true);

addLoadEvent(function () {

    function RoxyFileBrowser(field_name, url, type, win) {
        //alert('hi1');
        var roxyFileman = '/lib/fileman/index.html';
        if (roxyFileman.indexOf("?") < 0) {     
        roxyFileman += "?type=" + type;   
        }
        else {
        roxyFileman += "&type=" + type;
        }
        roxyFileman += '&input=' + field_name + '&value=' + win.document.getElementById(field_name).value;
        if(tinyMCE.activeEditor.settings.language){
        roxyFileman += '&langCode=' + tinyMCE.activeEditor.settings.language;
        }
        tinyMCE.activeEditor.windowManager.open({
            file: roxyFileman,
            title: __FileManager,//'Roxy Fileman',
            width: 850, 
            height: 650,
            resizable: "yes",
            plugins: "media",
            inline: "yes",
            close_previous: "no"  
        }, {     window: win,     input: field_name    });
        return false; 
    }
    
    
    //tfm_path = '/FileManager';
/* tinymce last version
    tinymce.init({
        selector: '#PageContentHTML',
        width: "100%",
        height: 500,
        theme: 'modern',
        //plugins: 'print preview fullpage powerpaste searchreplace autolink directionality advcode visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists textcolor wordcount tinymcespellchecker a11ychecker imagetools mediaembed  linkchecker contextmenu colorpicker textpattern help',
        plugins: 'print preview fullpage searchreplace autolink directionality visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists textcolor wordcount imagetools contextmenu colorpicker textpattern help',
        toolbar1: 'formatselect | bold italic strikethrough forecolor backcolor | link | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent  | removeformat',
        image_advtab: true,
        fontsize_formats: "8px 10px 12px 14px 18px 24px 36px 48px 62px",

        language: 'fa_IR',
        //language: 'ir',
        directionality: 'rtl',
        theme_advanced_toolbar_align: 'right',

        file_browser_callback: RoxyFileBrowser,
        templates: [
          { title: 'Test template 1', content: 'Test 1' },
          { title: 'Test template 2', content: 'Test 2' }
        ],
        content_css: [
          '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
          '//www.tinymce.com/css/codepen.min.css'
        ]
    });
*/

    tinymce.init({
        selector: '#PageContentHTML',
        width: "100%",

        script_url: '/plugin/tinymce/tinymce.js',
        selector: '#PageContentHTML',
        theme: "modern",
        height: 500,
        width: "100%",
        plugins: [
            "advlist autolink lists link image charmap print preview hr anchor pagebreak",
            "searchreplace wordcount visualblocks visualchars code codemirror fullscreen",
            "insertdatetime media nonbreaking save table contextmenu directionality",
            "emoticons template paste textcolor"
        ],
        toolbar1: "insertfile undo redo | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
        toolbar2: "print preview media | forecolor backcolor emoticons | ltr rtl | code fullscreen",
        toolbar3: "bold italic | fontselect |  fontsizeselect | styleselect ",


        image_advtab: true,

        //content_css: "css/custom_content.css",
        //theme_advanced_font_sizes: "10px,12px,13px,14px,16px,18px,20px",
        //font_size_style_values: "10px,12px,13px,14px,16px,18px,20px",
        //theme_advanced_fonts: "Yekan=yekan;Arial=arial;",
        //font_formats: "Yekan=web_yekan;",

        style_formats: [
            {
                title: 'Headers', items: [
                { title: 'Header 1', format: 'h1' },
                { title: 'Header 2', format: 'h2' },
                { title: 'Header 3', format: 'h3' },
                { title: 'Header 4', format: 'h4' },
                { title: 'Header 5', format: 'h5' },
                { title: 'Header 6', format: 'h6' }
                ]
            },
            {
                title: 'Inline', items: [
                { title: 'Bold', icon: 'bold', format: 'bold' },
                { title: 'Italic', icon: 'italic', format: 'italic' },
                { title: 'Underline', icon: 'underline', format: 'underline' },
                { title: 'Strikethrough', icon: 'strikethrough', format: 'strikethrough' },
                { title: 'Superscript', icon: 'superscript', format: 'superscript' },
                { title: 'Subscript', icon: 'subscript', format: 'subscript' },
                { title: 'Code', icon: 'code', format: 'code' }
                ]
            },
            {
                title: 'Blocks', items: [
                { title: 'Paragraph', format: 'p' },
                { title: 'Blockquote', format: 'blockquote' },
                { title: 'Div', format: 'div' },
                { title: 'Pre', format: 'pre' }
                ]
            },
            {
                title: 'Alignment', items: [
                { title: 'Left', icon: 'alignleft', format: 'alignleft' },
                { title: 'Center', icon: 'aligncenter', format: 'aligncenter' },
                { title: 'Right', icon: 'alignright', format: 'alignright' },
                { title: 'Justify', icon: 'alignjustify', format: 'alignjustify' }
                ]
            },
            {
                title: 'فونت فارسی', items: [
                   { title: 'یکان', inline: 'span', styles: { 'font-family': 'web_yekan' } },
                   { title: 'نازنین', inline: 'span', styles: { 'font-family': 'BNazanin' } },
                   { title: 'DroidKufi', inline: 'span', styles: { 'font-family': 'DroidKufi' } },
                   { title: 'Tahoma', inline: 'span', styles: { 'font-family': 'tahoma' } },
                   { title: 'Arial', inline: 'span', styles: { 'font-family': 'arial' } }
                ]
            }
        ],
 
        fontsize_formats: "8px 10px 12px 14px 18px 24px 36px 48px 62px",


        language: __langMark,//'fa',
        directionality:  __langDir,//'rtl',
        theme_advanced_toolbar_align: __langToolDir,//'right',

        file_browser_callback: RoxyFileBrowser,


        codemirror: {
            indentOnInit: true, // Whether or not to indent code on init.
            path: 'codemirror', // Path to CodeMirror distribution
            config: {           // CodeMirror config object
                mode: 'application/x-httpd-php',
                lineNumbers: false
            },
            jsFiles: [          // Additional JS files to load
               'mode/clike/clike.js',
               'mode/php/php.js'
            ]
        },


        templates: [
            { title: 'Test template 1', content: 'Test 1' },
            { title: 'Test template 2', content: 'Test 2' }
        ]

    });




    var jvalidate = $("#page-form").validate({
        //debug: true,
        rules: {
            PageName: {
                required: true
            },
            PageTopCode: "check_val"           
        },
        messages: {
            PageName: {
                required: __val_pageName
            }
        }
    });

    jQuery.validator.addMethod("check_val", function(value) {
        var text = tinyMCE.get('PageContentHTML').getContent({format : 'text'});
        //alert(text);
        $("#PageContentText").val(text);

        return true; }, 
        ''
    );


    $('#reset-form').click(function () {
        resetForm($("#page-form"));

        $('#page-msg').text('');
        $('#page-msg').hide();
    });

    $('#back-pg-btn').click(function () {
        $.ajax({
            type: 'POST',
            url: '../Pages/CancelPage',
            contentType: 'application/x-www-form-urlencoded',
            data: { pathUnicId: $('input[name=NewPath]').val() },

            success: function (redirect) {
                //window.location = redirect;
                window.location.replace(redirect);
            }
        });

        //window.location.replace('/Admin/'+__langMark2+'/Pages/');
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

function onBeginSave(){
    $('#create-page-btn').attr({ 'disabled': 'disabled', 'value': __submiting });

    $('#dialogWait').modal({ keyboard: false, backdrop: 'static' });
    $('#page-msg').hide();
}

function onFailedSave(data){
    $('#dialogWait').modal('hide');
    $('#page-msg').text("Failed /n"+data);
    $('#page-msg').show("slow");
    $('#create-page-btn').removeAttr("disabled").attr({'value': __create });
}

function onSuccessSave(data){
    if (data.res.toString().toLowerCase().trim() == "ok") {
        alert(data.msg);
        window.location.replace('/admin/'+__langMark2+'/Pages/');
        return;
    } else {
        $('#create-page-btn').removeAttr("disabled").attr({'value': __create });

        $('#page-msg').text(data.msg);
        $('#page-msg').show("slow");
    }
    $('#dialogWait').modal('hide');
}
