function DetelePageGroup(idPG, namePG) {
    if (!confirm(__mgDeletePG))
        return false;

    $('#hfDeletePGId').val(idPG.toString());
    $('#pagegroup-delete-form').submit();
}

function onSuccessDelPG(event) {
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

function onBeginDelPG() {
    $('#dialogWait').modal({ keyboard: false, backdrop: 'static' });
}

function onFailedDelPG(event) {
    $('#dialogWait').modal('hide');
    alert(event.msg);
}
