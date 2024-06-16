$('#back-homepage').click(function () {
    window.location.replace('/'+__langMark+'/');
});

$('#btn-logout').click(function () {
    window.location.replace('/'+__langMark+'/Users/Logout');
});

$('#btn-admin').click(function () {
    window.location.replace('/Admin/'+__langMark+'/Home/Index');
});
