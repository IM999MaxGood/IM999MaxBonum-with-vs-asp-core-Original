addJsEnd('/plugin/jquery-validation/jquery.validate.min.js', true);

addJsEnd("/plugin/treeview/jstree.js", true);
addCss("/plugin/treeview/themes/default-dark/style.min.css", true);

//addJsEnd("/plugin/context-menu/jquery.contextMenu.js", true);
//addCss("/plugin/context-menu/jquery.contextMenu.css", true);

addLoadEvent(function () {

    $('#tree-view-menu').jstree({
        'core': {
            "themes": {
                "name": "default-dark",
                "dots": true,
                "icons": true
            }
        }
    });

    $('#tree-view-menu').jstree('open_all');
    
    //initContextMenu();

    var jvalidate = $("#menu-form").validate({
        //debug: true,
        rules: {
            MenuName: {
                required: true
            }
        },
        messages: {
            MenuName: {
                required: __val_menuName
            }
        },

    });

    $('#reset-form').click(function () {
        resetForm($("#menu-form"));

        $('#tree-view-menu ul').html('');
        //$('#create-root-btn').show();
        
        $('#menu-msg').text('');
        $('#menu-msg').hide();
    });

    $('#back-pg-btn').click(function () {
        window.location.replace('/Admin/'+__langMark+'/Menus/');
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
    $('#menu-msg').hide();
}

function onFailedCreate(data){
    $('#dialogWait').modal('hide');
    $('#menu-msg').text("Failed /n"+data);
    $('#menu-msg').show("slow");
    $('#create-pg-btn').removeAttr("disabled").attr({'value': __create });
}

function onSuccessCreate(data){
    if (data.res.toString().toLowerCase().trim() == "ok") {
        alert(data.msg);
        window.location.replace('/admin/'+__langMark+'/Menus/');
        return;
    } else {
        $('#create-pg-btn').removeAttr("disabled").attr({'value': __create });

        $('#menu-msg').text(data.msg);
        $('#menu-msg').show("slow");
    }
    $('#dialogWait').modal('hide');
}

/*
function initContextMenu() {
    function DoContextMenuMenu(key, options) {
        //alert('key:' + key.toString());
        //alert('options:' + options.selector.toString());
        //alert($(options.selector.toString()).attr('id'));            

        var x = $(options.selector.toString()).attr('id');
        var menuName = $(options.selector.toString()).text();
        switch (key) {
            
            case 'addParent':
                
                //$('#hfMenuId').val('');
                //$('#hfParentInsert').val('1');
                //$('#hfMenuParentId').val(x);
                //javascript: DoEditMenu(true);

                $('#MenuName').val('');
                $('#MenuUrl').val('');
                $('#hfMenuId').val('0');
                $('#hfMenuParentId').val('0');
                $('#input-menu').modal('show');


                break;
            
            case 'addChild':
                $('#MenuName').val('');
                $('#MenuUrl').val('');
                $('#hfMenuId').val('0');
                $('#hfMenuParentId').val(x);
                $('#input-menu').modal('show');
                break;
            case 'edit':
                $('#hfMenuId').val(x);
                $('#MenuName').val($(options.selector.toString()).text());
                $('#MenuUrl').val($(options.selector.toString()).attr('data-url'));
                $('#hfMenuParentId').val($(options.selector.toString()).attr('data-perent-id'));
                $('#input-menu').modal('show');
                break;
            case 'del':
                javascript: deleteMenu(x, menuName);
                break;
        }
    }


    var countMenu = $("[class*=ContextMenu_]").length;

    for (i = 0; i < countMenu; i++) {
        var nodeId = $("[class*=ContextMenu_]")[i].id.toString();
        $.contextMenu({
            selector: ".ContextMenu_" + nodeId,
            items: {
                "addParent": { name: "اضافه کردن سرمنو", icon: "insert", callback: function (key, options) { DoContextMenuMenu(key, options); } },
                "addChild": { name: "@Resource.Add @Resource.SubMenu", icon: "insert", callback: function (key, options) { DoContextMenuMenu(key, options); } },
                "edit": { name: "@Resource.Edit @Resource.Menu", icon: "editing", callback: function (key, options) { DoContextMenuMenu(key, options); } },
                "del": { name: "@Resource.Delete @Resource.Menu", icon: "del", callback: function (key, options) { DoContextMenuMenu(key, options); } }
            }
        });
    } //for


} //func initContextMenu
*/

/******************* */
$('#cancel-btn').click(function () {
    $('#input-menu').modal('hide');
    $('#MenuName').val('');
    $('#MenuUrl').val('');
});

$('#create-btn').click(function () {
    var name = $('#MenuName').val();
    var url = $('#MenuUrl').val();
    if (name != '' & url != '') {
        $('#input-menu').modal('hide');
        createRootMenu_(url, name);
    }
    $('#MenuName').val('');
    $('#MenuUrl').val('');
});

/**************************** */

function createRootMenu(){
    $('#input-menu').modal('show');
}

function createRootMenu_(_menuUrl, _menuName)
{
    alert($('#tree-view-menu ul').html());
    $('#tree-view-menu ul').append('<li><a data-url="'+_menuUrl+'">'+_menuName+'</a></li>');
    alert($('#tree-view-menu ul'));
    alert($('#tree-view-menu ul').html());
    //$('#create-root-btn').hide();

/*
<li>
    <a id="@item.MenuId" data-perent-id="@item.MenuParentId" data-url="@item.MenuUrl" class="ContextMenu_@item.MenuId">@item.MenuName</a>
*/
}