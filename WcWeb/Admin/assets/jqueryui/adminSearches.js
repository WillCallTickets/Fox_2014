

//the following three functions handle the _nameEditors Act_Editor control
actEditorSearch = function () {

    var tts = $('.typeahead[data-searchtype]').each(function (idx) {
        var self = $(this);
        //examine the data-rel
        var rel = self.attr("data-searchtype");
        assignTypeAhead_ActEditor(self, rel)
    });
}
//currently just handles act functionality
assignTypeAhead_ActEditor = function (e, ctx) {
    
    var lmt = (ctx == 'Show') ? 50 : 25;

    var results = new Bloodhound({
        datumTokenizer: function (d) { return Bloodhound.tokenizers.whitespace(d.Suggestion); },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        limit: lmt,
        remote: { url: '/api/searches/admin/' + ctx + '/%QUERY/' + lmt + '/0' }
    });

    results.initialize();

    e.typeahead(
    {
        hint: true,
        minLength: 2,
        highlight: true,
    },
    {
        name: 'admin-searches',
        displayKey: 'Suggestion',
        source: results.ttAdapter()
    }).on('typeahead:selected', function (e, datum) {
        //do nothing 
        var idx = datum.Id;

        var container = getPostbackContainerIdHelper(e.currentTarget.name, 1);
        postBack_ToControl_WithArgs(container, { "commandName": 'selectedidfromtypeahead', "newIdx": idx });

    })//.on('blur', function (e) {

    //    if(ctx != 'Show' && ctx != 'Act' && ctx != 'Promoter' && ctx != '')
    //        return sendSelectdInputBackToServerControl(e.currentTarget, ctx);

    //})
        .keypress(function (e) {
        if (ctx != 'Show') {
            var keycode = (e.keyCode ? e.keyCode : e.which);
            if (keycode == '13' && ctx != 'Show') {
                $(this)[0].blur();
            }
        }
    });
}
sendSelectdInputBackToServerControl = function (control, ctx) {

    if (control.value.trim().length == 0)
        return true;

    //if no match - offer to quick add (offer edit/save)

    //get container from name of this control
    //name="ctl00$MainContent$ctl01$formEnt$Act_Editor1$hdnCollectionTableName"
    //alert(container);//test for diff browsers?ie, chrome, ff, safari - ok
    var container = control.name.replace('$' + control.id, '');

    //check the entry
    $.ajax({
        url: '/Admin/_customControls/NamedEntityOperations.ashx',
        data: {
            'commandOperation' : "verifyexistence",
            'ctx' : ctx,
            'input' : control.value
        },
        datatype: "text",//handler is text
        type: 'post',

        success: function (result) {
            if (result.indexOf("Error: ") != -1)
                alert(result);
            else {
                //insert url into click textbox
                var res = JSON.parse(result);
                if (res["status"] == 'true') {//converted to lower case in handler!!
                    //do nothing //alert('keep it');
                }
                else {
                    var con = confirm('This ' + ctx + ' does not exist. Would you like to create a new ' + ctx + '?');

                    if (con) {
                        //alert('show new interface');
                        postBack_ToControl_WithArgs(container, { "commandName": 'showNewNamedEntityInterface', "newName": control.value });
                    }
                    else {
                        //do nothing?
                    }
                }

                return true;
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {

            alert(ctx + ' info retrieval Error :' + xhr.responseText);
        }
    });

    return true;
}


registerAdminSearches = function () {

    //for show searches within controls - different functionality
    actEditorSearch();


    //twitter typeahead
    var tts = $('#coll-searchterms');

    if (tts != undefined && tts.length > 0) {

        var hdns = $('#hdnCollectionTableName');

        var ctx = $('#hdnCollectionTableName').val();
        var lmt = (ctx == 'Show') ? 50 : 15;

        var results = new Bloodhound({
            datumTokenizer: function (d) { return Bloodhound.tokenizers.whitespace(d.Suggestion); },
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            limit: lmt,
            remote: { url: '/api/searches/admin/' + ctx + '/%QUERY/' + lmt + '/0' }
        });

        results.initialize();


        tts.typeahead(
            {
                hint: true,
                minLength: 2,
                highlight: true,
            },
            {
                name: 'admin-searches',
                displayKey: 'Suggestion',
                //valueKey: (ctx == 'Show') ? 'Id' : 'Suggestion',//just use datum on selection
                source: results.ttAdapter()
            }
        ).on('typeahead:selected', function (event, datum) {

            //When a typeahead result is selected, we should find the entity by the id
            // of the selection. Postback to the containing control to do the FindById
            // and assign to the current object

            var idx = datum.Id;

            if (ctx == 'Show') {
                postBackToShowSelector({ "commandName": 'selectedIdFromTypeahead', "newIdx": idx });
            }
            else
                postBackWithArgs({ "commandName": 'selectedIdFromTypeahead', "newIdx": idx });


            //no need to click!
            //$('#coll-sitesearch').click();

            //clear the search box
            $('.typeahead').typeahead('val', '').blur();
            

        }).keypress(function (event) {

            //On a keypress, we should use the text in the input for search terms
            //this should be ignored when the tableName is Show, Genre, (Act and Venue?)


            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13' && ctx != 'Show') {

                $('#coll-sitesearch').click();
                //clear the search box
                $('.typeahead').typeahead('val', '').blur();
            }
        });

        //this button has been removed from the pager
        //but leaving this script for future use
        $('.search-term-reset').on('click', function (event) {
            $('#coll-sitesearch').click();
            event.stopPropagation();
        });

        $('#coll-sitesearch').on('click', function () {

            //do we have a value in the search box at this point?
            var searchTerms = $('#coll-searchterms').val();

            //reset the cookie to have search values - note we are changing to an ALL criteria as well
            // and changing search dates
            $.cookie.raw = true;//turn off encoding

            var cooKey = _getCooKeyFromContext('AdminCollectionCriteriaCookie', ctx);
            var newCriteria = 'a~2,sd~,ed~,t~' + searchTerms;//all is 2

            //overwrite/create cookie
            //must explicitly use path to keep cookies in sync
            $.cookie(cooKey, newCriteria, { path: '/' });

            //reset encoding
            $.cookie.raw = false;

            //postback and bind
            postBackToParent('rebind');

            //clear the search box
            $('.typeahead').typeahead('val', '').blur();

        });
    }
    //END TWITTER TYPEAHEAD
}
