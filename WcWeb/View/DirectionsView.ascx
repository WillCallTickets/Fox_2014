<%@ Control Language="C#" AutoEventWireup="false" CodeFile="DirectionsView.ascx.cs" Inherits="wctMain.View.DirectionsView" %>
    
<style type="text/css">
    .directions-container img{max-width:none}
</style>

<script type="text/javascript">

    var foxpoint;
    var map;
    var marker;
    var infowindow;

    function initializeGMap() {

        //TODO: update from wikipedia? get new coords
        foxpoint = new google.maps.LatLng(40.0080, -105.2764);
        var mapOptions = {
            center: foxpoint,                
            zoom: 17,
            mapTypeId: google.maps.MapTypeId.HYBRID
        };
        map = new google.maps.Map(document.getElementById("map_canvas"),
            mapOptions);
        marker = new google.maps.Marker({
            position: foxpoint,
            map: map,
            animation: google.maps.Animation.DROP
        });    
    }

    function toggleBounce() {

        if (marker.getAnimation() != null) {
            marker.setAnimation(null);
        } else {
            marker.setAnimation(google.maps.Animation.BOUNCE);
        }
    }

    function loadGMap() {
      
        var script = document.createElement("script");
        script.type = "text/javascript";
        script.src = "http://maps.googleapis.com/maps/api/js?key=AIzaSyBTS8Tz-MWiBGEy9lUwmmJ9iqA4N-2GwpY&sensor=true&callback=initializeGMap";
        document.body.appendChild(script);
    }

    function setGMapDirections(fromAddress, toAddress) {

        //clear any existing directions
        $('#directions_panel').html('');

        if (fromAddress.trim().length > 0) {

            var directionsDisplay = new google.maps.DirectionsRenderer();
            var directionsService = new google.maps.DirectionsService();

            directionsDisplay.setMap(map);
            directionsDisplay.setPanel(document.getElementById('directions_panel'));

            var request = {
                origin: fromAddress,
                destination: toAddress,
                travelMode: google.maps.TravelMode.DRIVING
            };

            directionsService.route(request, function (response, status) {
                if (status == google.maps.DirectionsStatus.OK) {
                    directionsDisplay.setDirections(response);
                }
                else {
                    $('#directions_panel').html('<div class=\"text-error\">We are unable to process your request. Please check the address provided.</div>');
                }
            });
        }
    }
    
    $(document).ready(function () {

        loadGMap();
    });

</script>

<div class="displayer fade fade-slow directions-container static-container">
    <div class="section-header">Directions</div>
    <form id="directionsform" action="directions" runat="server" role="form" class="form-horizontal main-inner">  
        <div class="form-group">
            <address>
                <div><%=Wcss._Config._Site_Entity_PhysicalAddress %></div>
                <div>Main Office: <%=Wcss._Config._MainOffice_Phone %></div>
                <div>Box Office: <%=Wcss._Config._BoxOffice_Phone %></div>
            </address>
            <div id="map_canvas" style="margin-bottom:12px;width: 100%; height: 320px;">
                <div class="spinner-loader"><span class='wct-modal-loader-spinner'></span>Loading Google Maps...</div>
            </div>        
        </div>
        <div class="form-group">
            <input type="text" id="fromAddress" name="fromAddress" class="form-control" placeholder="Your address (use zip or city, state) then click directions" />
        </div>
        <div class="form-group">
            <button id="submitAddress" name="submitAddress" type="submit" class="btn btn-foxt"
                onclick="setGMapDirections(fromAddress.value, toAddress.value); return false;" >Get Directions!</button>
        </div>
        <div class="form-group">
            <input type="hidden" id="toAddress" name="toAddress" value="1135 13th Street Boulder, CO 80302" />    
            <div id="directions_panel" style="width: 100%"></div>
        </div>
    </form>
</div>
