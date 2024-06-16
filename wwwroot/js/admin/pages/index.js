function DetelePage(idP, nameP) {
    if (!confirm(__mgDeleteP +"\'"+nameP+"\' ?"))
        return false;

    $('#hfDeletePId').val(idP.toString());
    $('#page-delete-form').submit();
}

function onSuccessDelP(event) {
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

function onBeginDelP() {
    $('#dialogWait').modal({ keyboard: false, backdrop: 'static' });
}

function onFailedDelP(event) {
    $('#dialogWait').modal('hide');
    alert(event.msg);
}
