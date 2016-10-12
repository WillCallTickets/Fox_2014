
String.prototype.trim = function() {
    return this.replace(/^\s+|\s+$/g, "");
}
String.prototype.ltrim = function() {
    return this.replace(/^\s+/, "");
}
String.prototype.rtrim = function() {
    return this.replace(/\s+$/, "");
}

function showCoords(c) {
    // variables can be accessed here as
    // c.x, c.y, c.x2, c.y2, c.w, c.h
    
    jQuery('#x1').val(c.x);
    jQuery('#y1').val(c.y);
    jQuery('#x2').val(c.x2);
    jQuery('#y2').val(c.y2);
    jQuery('#w1').val(c.w);
    jQuery('#h1').val(c.h);
    
};

function ValidateSubscriptionSend(domClock) {
    
    var elem = getElementByLikeId(document.forms[0], domClock);

    if (elem.value == '') {
        alert('You have not selected a valid date for the mailer to be sent.');
        return false;
    }
    else {
        var dt = elem.value;//mm/dd/yyyy
        var pieces = dt.split('/');

        var hour = getElementByLikeId(document.forms[0], domClock + '_ddlHour');
        var minu = getElementByLikeId(document.forms[0], domClock + '_ddlMinute');
        var ampm = getElementByLikeId(document.forms[0], domClock + '_ddlAmpm');
        var hour24 = (ampm.value == 'PM') ? (parseInt(hour.value) + 12).toString() : hour.value;

        //construct date from the inputs
        var time = hour.value + ':' + minu.value + ' ' + ampm.value;
        var datestring = pieces[2] + '-' + pieces[0] + '-' + pieces[1];
        var datetime = new Date(datestring);
        datetime.setHours(0, 0, 0);

        //compare to current date
        var today = new Date();
        today.setHours(0, 0, 0);

        var extralert = '';
        if (datetime > today) {
            var mildif = datetime.getTime() - today.getTime();
            var days = Math.ceil(mildif / (1000 * 60 * 60 * 24));
            if (days > 10) {
                extralert = 'ALERT!!! You have selected to send the mailer ' + days + ' days in the future! ';
            }
        }

        //indicate how far in advance this is and ask for confirm
        return confirm(extralert + 'You have chosen to send the mailer on ' + dt + ' at ' + time + '. Please confirm that you have reviewed the recipients and that this time is correct');
    }

    return false;
}


function FCKUpdateLinkedField(id) {
    try {
        if (typeof (FCKeditorAPI) == "object") {
            FCKeditorAPI.GetInstance(id).UpdateLinkedField();
        }
    }
    catch (err) {
    }
}


function allowNoMoreThan_N_Selections(sender, maxSelections) {

    var baseName = sender.name.substring(0, sender.name.length-1);

    //find the other checkboxes with the like id
    //regex
    //var baseId = baseName.replace(/\$/g, "_");
    var checks = new Array();
    
    //loop thru the elements and get all of the checkbox elements
    for (var i = 0; i < theForm.length; i++) {

        var theElement = theForm.elements[i];
        if (theElement.type != null && theElement.type == "checkbox" && theElement.name.indexOf(baseName) != -1 && theElement.checked) {
            checks.push(theElement);
        }
    }
    
    //if we are over selected - than this element cannot be selected
    //deselect and show alert?
    if (checks.length > maxSelections)
        sender.checked = false;
}

function showEmailLetterSelection(ddlName) 
{
    var elem = getElementByLikeId(document.forms[0], ddlName);
    
    if(elem != undefined) {
    
        if(elem.value != 0)
            doPagePopup('/Admin/_mailer/MailerViewer.aspx?mlr=' + elem.value,'true')
    }
}

function FillTextBoxWithText(controlName, txt)
{
    var elem = getElementByLikeId(document.forms[0],controlName);
    
    if(elem != undefined) {
    
        elem.value = txt;
    }
}

function imagePopup(imageId, dimension) {
    
    try {
        var width = 48; //offset - a nice safe number
        width += parseInt(dimension,10);
        
        var URL = "/ImagePopup.aspx?img=" + imageId;
        
	    H = window.open("", 'Information', 'toolbar=0,scrollbars=1,location=0,status=0,menubar=0,resizable=1,width=' + width + ',height=' + dimension + ',screenX=100,screenY=200,top=100,left=200');
	    H.location.href = URL;
	    H.focus(); 
	}
	catch(ex) {}
}

function doPagePopup(url, employBrowserFeatures) {
    
	try {
		var H
		if(employBrowserFeatures == undefined || employBrowserFeatures == 'false')
		    H = window.open("", 'Information', 'toolbar=1,scrollbars=1,location=0,status=0,menubar=0,resizable=1,width=1000,height=800,screenX=50,screenY=50,top=50,left=50');
		else
		    H = window.open("", 'Information', 'toolbar=1,scrollbars=1,location=1,status=1,menubar=1,resizable=1,width=1000,height=800,screenX=50,screenY=50,top=50,left=50');
		H.location.href = url;
		H.focus(); 
	}
	catch(Exception) {}
}

function doPagePopupWithDimension(url, width, height) {
	try {
		
		var H = window.open("", 'Information', 'toolbar=0,scrollbars=0,location=1,status=0,menubar=0,resizable=1,width=' +  width + ',height=' + height + ',screenX=50,screenY=50,top=50,left=50');
		H.location.href = url;
		H.focus(); 
	}
	catch(Exception) {}
}


function Set_Cookie( name, value, expires, path, domain, secure ) {

	// set time, it's in milliseconds
	

	/*
	if the expires variable is set, make the correct 
	expires time, the current script below will set 
	it for x number of days, to make it for hours, 
	# * 24, for minutes, delete * 60 * 24
	*/
    if (expires) {
        var expiryDate = new Date();
        expiryDate.setTime(today.getTime());

        var tmp = expires * 1000 * 60 * 60 * 24;

	    expires = new Date(expiryDate.getTime() + (tmp));
	}

	document.cookie = name + "=" +escape( value ) + ( ( expires ) ? ";expires=" + expires.toGMTString() : "" ) + 
	( ( path ) ? ";path=" + path : "" ) + ( ( domain ) ? ";domain=" + domain : "" ) + ( ( secure ) ? ";secure" : "" );
}


// this function gets the cookie, if it exists
function Get_Cookie( name ) {
	
	
	var start = document.cookie.indexOf( name );//+ "=" );
	//alert("gc_start: " + start);
	
	var len = start + name.length + 1;
	
	if ( ( !start ) && ( name != document.cookie.substring( 0, name.length ) ) ) {
		//alert("gc_not found");
		return null;
	}
	
	if ( start == -1 ) {
		//alert("gc_not found 2");
		return null;
	}
	
	var end = document.cookie.indexOf( ";", len );
	
	if ( end == -1 ) end = document.cookie.length;
	
	
	var retval = unescape( document.cookie.substring( len, end ) );
	
	alert("retval: " + retval);
	
	return retval;
}


// this deletes the cookie when called
function Delete_Cookie( name, path, domain ) {
	
	if ( Get_Cookie( name ) ) 
		document.cookie = name + "=" + ( ( path ) ? ";path=" + path : "") + ( ( domain ) ? ";domain=" + domain : "" ) + 
		";expires=Thu, 01-Jan-1970 00:00:01 GMT";
}


function ValidateRequiredCheckBox(source,args) {

	//source is the custom validator	
	if(source.id.indexOf('CustomTerms') != -1) {
		var elem = getElementByLikeId(document.forms[0],'CheckTerms');
		
		if(elem != undefined) {
			args.IsValid = elem.checked;
		}
		
	}
}

 //this works on controls that have been named with an underscore
function getDOMElement(parentId, controlToFind) {
    
    if(parentId == undefined)
        return "";
    
    var idParts;
    
    if(parentId.indexOf("$") != -1)
        idParts = parentId.split('$');
    else
        idParts = parentId.split(':');
    
    //idParts.pop();//get rid of the last element - the parent id name
    idParts.push(controlToFind);
    //return uniqueID.replace(/\$/g, '_');
    var controlName = idParts.join("_");
    var ret = $get(controlName);
    
    if (ret == null || ret == undefined) {
        ret = $get(controlToFind);
    }

    return ret;
}
function getChildElement(parentId, controlToFind) { 
    
    if(parentId == undefined)
        return "";
    
    var idParts;
    
    if(parentId.indexOf("$") != -1)
        idParts = parentId.split('$');
    else
        idParts = parentId.split(':');
    
    idParts.pop();//get rid of the last element - the parent id name
    idParts.push(controlToFind);
    var controlName = idParts.join("_");
    var ret = $get(controlName);

    if (ret == null || ret == undefined) {
        ret = $get(controlToFind);
        //alert(ret);
    }
    
    return ret;
}

function redirect(url) {
    window.location = url;
}
    
	
//Use this one		
function getElementByLikeId(form,elementName) {
	
	var theForm = form;
	
	for(var i=0; i<theForm.length; i++) {
		
		var theElement = theForm.elements[i];
		
		if ((theElement.type != null) && ((theElement.id.indexOf(elementName) != -1) || (theElement.name.indexOf(elementName) != -1)))
		{								
			return theElement;
		}
	}
	
	return undefined;
}

function EnsureShipCheck(sender, checkId) { 
    
    var checkbox = getElementByLikeId(document.forms[0], checkId);
    
    if(sender != undefined && checkbox != undefined) { 
        
        var input = sender.value;
        if(input != undefined && input.length > 0)
            checkbox.checked = false;
    }
}


function SetFieldFocus(source, fieldToFocus) {

	getElementByLikeId(source.document.forms[0], fieldToFocus).focus();
	
}


function setFieldFocus(strForm, fieldToFocus) {

	getElementByLikeId(strForm, fieldToFocus).focus();
	
}

function set_FieldFocus(strField) { 

	var frm = document.forms[0];
	getElementByLikeId(frm, strField).focus();
}

function CloseTheWindow() {
	
	try
	{
		netscape.security.PrivelegeManager.enablePrivelege("UniversalBrowserWrite");
		
	}
	catch(Exception){}
	
	self.close();
}
			