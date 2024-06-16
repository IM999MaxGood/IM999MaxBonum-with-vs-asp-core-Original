function DeleteMenu(idMenu, nameMenu) {
    if (!confirm(__mgDeleteMenu))
        return false;

    $('#hfDeleteMenuId').val(idMenu.toString());
    $('#menu-delete-form').submit();
}

function onSuccessDelMenu(event) {
    if (event.res.toString().toLowerCase().trim() == "ok") {
        alert(event.msg);
        //if (event.url)
        //    window.location = event.url;
        //else
            location.reload(true);
        return;
    } else {
        alert(event.msg);
    }
    $('#dialogWait').modal('hide');
}

function onBeginDelMenu() {
    $('#dialogWait').modal({ keyboard: false, backdrop: 'static' });
}

function onFailedDelMenu(event) {
    $('#dialogWait').modal('hide');
    alert(event.msg);
}
