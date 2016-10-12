

//javascript to handle show editing functions
//this is the entry point rtegistered with wctadmin
function registerShowMgmt() {

    var showMgr = $('.show-manage-control');//director
    //var showMgr = $('.show-manage-select');//director

    if (showMgr.length == 1) {
        newShowListControls(showMgr);
    }
}


function newShowListControls(showMgr) {

    //find the principal control
    //var rdoPrince = $(showMgr).find('#rdoPrincipal');
    
    //$(rdoPrince).find('input').click(function (e) {        //fillVenueList();

    //    var container = getPostbackContainerIdHelper($(this)[0].name, 2);
    //    postBack_ToControl_WithArgs(container, { "commandName": 'reloadVenueListing', "emptyparam": '' });
    //});
}









