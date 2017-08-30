//var map = null;
//var xmin, xmax, ymin, ymax;
//function GetMap() {

    
//    if (xmax && xmin && ymax && ymin) {
//        var x = (xmax + xmin) / 2;
//        var y = (ymax + ymin) / 2;

//        var ll = new  Microsoft.Maps.Location(y, x);
//        //alert(x +","+y);
//        //map.SetDashboardSize(VEDashboardSize.Small);
//        zoom = getZoom(xmin, xmax, ymin, ymax);
//        //alert (zoom); 
//        //map.LoadMap(ll, zoom, 'h', false);
//        map = new Microsoft.Maps.Map('#myMap', {
//            credentials: 'AqptI9yjcnFDAyDw-5RsaxS_4ykE0NqvhqHkU8IcGOsL8882CmH-ruaHW2JK6ioA',
//            center: ll,
//            zoom:zoom
//        });

//        //alert(); 
//        addShape(xmin, xmax, ymin, ymax);//",10);
//    } else {
//        map.LoadMap();
//    }

//}

//function getZoom(x1, x2, y1, y2) {
//    if (x1 != x2 && y1 != y2) {
//        w = (x2 - x1) > (y2 - y1) ? x2 - x1 : y2 - y1;
//        //alert (w);
//        if (w > 60) {
//            return 2;
//        } else if (w > 28) {
//            return 3;
//        } else if (w > 14) {
//            return 4;
//        } else if (w > 7) {
//            return 5;
//        } else if (w > 3.5) {
//            return 4;
//        } else if (w > 1.8) {
//            return 7;
//        } else if (w > 0.9) {
//            return 8;
//        } else if (w > 0.5) {
//            return 9;
//        } else if (w > 0.2) {
//            return 10;
//        } else if (w > 0.11) {
//            return 11;
//        } else if (w > 0.05) {
//            return 12;
//        } else {
//            return 10;
//        }

//    } else {
//        return 10;
//    }
//}

//function addShape(x1, x2, y1, y2) {
//    if (x1 != x2 && y1 != y2) {
//        //alert(x1 +","+y1+"  "+x2+","+y2);
//        try {
//            var p1 = new  Microsoft.Maps.Location(y1, x1);
//            var p2 = new  Microsoft.Maps.Location(y1, x2);
//            var p3 = new Microsoft.Maps.Location(y2, x2);
//            var p4 = new Microsoft.Maps.Location(y2, x1);
//            //zbshape = new Microsoft.Maps.(VEShapeType.Polygon, new Array(p1, p2, p3, p4));
//            var polylineColor = new Microsoft.Maps.Color(0.39, 100, 0, 100);
//            var zbshape = new Microsoft.Maps.Polygon(new Array(p1, p2, p3, p4), { strokeColor: polylineColor });
//            map.entities.push(zbshape);
//            //zbshape.SetLineWidth(1);
//            //zbshape.SetLineColor(new VEColor(255, 100, 100, 1.0));
//            //zbshape.SetFillColor(new VEColor(255, 255, 255, 0.8));
//            //zbshape.HideIcon();
//            //map.AddShape(zbshape);
//        } catch (er) {
//            status = "exception: addzbshape: " + er.message;
//            alert(status);
//        }
//    }
//}  
     

    var map;
         var xmin, xmax, ymin, ymax;
         function initMap() {

             map = new google.maps.Map(document.getElementById('myMap'));
             map.setOptions({ maxZoom: 14 });
             map.setOptions({ minZoom: 1 });
    //alert(xmin + "," + xmax + "," + ymin + "," +ymax)
             xmin = Math.min(xmin, xmax);
             xmax = Math.max(xmin, xmax);
             ymin = Math.min(ymin, ymax);
             ymax = Math.max(ymin, ymax);

             //set maxx minx
             //if (xmax > 160) xmax = 160;
            // if (xmin < -160) xmin = -160;
             if (ymin < -80) ymin = -80;
             if (ymax > 80) ymax = 80;
             //Fit map to initial bounds...
             var bounds = new google.maps.LatLngBounds(
                 new google.maps.LatLng(ymin,xmin),	// SW corner of map
                 new google.maps.LatLng(ymax, xmax)	    // NE corner of map
             );

             var sum = Math.abs(xmin-xmax)  ;
             
             if (sum > 180 )
             {
                 addPolygontoMap(xmin, 0, ymin, ymax)
                 addPolygontoMap(0, xmax, ymin, ymax)
             
             }
             else{ addPolygontoMap(xmin, xmax, ymin, ymax)}

            

             map.fitBounds(bounds);
             //alert(map.getZoom())
            // if (map.getZoom() > 10) map.setZoom(12);
             //var path = getLatLngArraysForBounds(xmin,xmax,ymin,ymax)
             //if (xmin < 0)
             //{
             //   var sum = Math.sign(xmin) + xmax;
             //   alert(sum);
             //   if (sum > 180) {
             //       //xmin = xmin;
             //       xmax = 0;
             //   }
             //}
            
             




         }
         function addPolygontoMap(xmin, xmax, ymin, ymax)
        {
             var PolygonCoords = [
                 { lat: ymax, lng: xmax },
                 //{lat: ymax, lng: xmin /2 },
                 { lat: ymax, lng: xmin },
                 { lat: ymin, lng: xmin },
                 { lat: ymin, lng: xmax },
                 //{lat: ymax, lng: xmin },
                 { lat: ymax, lng: xmax }
             ];

             // Define the LatLng coordinates for the polygon's path.

             var rect = new google.maps.Polygon({
                 paths: PolygonCoords,
                 strokeColor: '#FF0000',
                 strokeOpacity: 0.4,
                 strokeWeight: 0,
                 fillColor: '#FF0000',
                 fillOpacity: 0.25
             });
             rect.setMap(map);
         }
         //Return one or more Google MVCArray<LatLng>s for the input LatLngBounds
         function getLatLngArraysForBounds(xmin, xmax, ymin, ymax) {
             var latlngNe = new google.maps.LatLng(ymax, xmax); //LatLng: north-east corner
             var latlngSw = new google.maps.LatLng(ymin, xmin); //LatLng: south-west corder

             var latlngNw = new google.maps.LatLng(latlngNe.lat(), latlngSw.lng()); //LatLng: north-west corner
             var latlngSe = new google.maps.LatLng(latlngSw.lat(), latlngNe.lng()); //LatLng: south-east corner

             console.log('********* Original Values ************');
             console.log('NorthEast: ' + latlngNe.toString());
             console.log('NorthWest: ' + latlngNw.toString());
             console.log('SouthWest: ' + latlngSw.toString());
             console.log('SouthEast: ' + latlngSe.toString());
             console.log('**************************************');

             //'Normalize' lng values, if indicated...
             //Measured from Greenwich (Lng: 0.0) - 'East' values range from 0 to POSITIVE (+)180
             //                                   - 'West' values range from 0 to NEGATIVE (-)180
             //
             //Thus: 'NorthEast-SouthEast' points with a NEGATIVE lng are in the 'west' (relative to Greenwich)
             //      'NorthWest-SouthWest' points with a POSITIVE lng are in the 'east' (relative to Greenwich)
             //      The rectangular area of the map can consist of up to three zones: 'west', 'center' and 'east'
             //
             //ASSUMPTION: The map never repeats any part of the globe, thus the lat/lng rectangle contains no overlaps

             //NOTE: For use with google.maps.geometry.spherical.computeArea(...)
             //       array elements MUST conform to the following order:
             //          NE -> NW -> SW -> SE
             // Source: http://stackoverflow.com/questions/16313411/two-similar-polygons-in-google-maps-have-very-different-areas
             var mvcArrays = {
            'west': new google.maps.MVCArray(),
                 'center': new google.maps.MVCArray(),
                 'east': new google.maps.MVCArray()
             };
             var mvcArray = null;
             //Check 'NorthWest-SouthWest' lng...
             var nwLng = latlngNw.lng();
             var swLng = latlngSw.lng();
             if (0 < nwLng && 0 < swLng) {
            //'NorthWest-SouthWest' points are in the 'east' (relative to Greenwich) 
            mvcArray = mvcArrays.east;

        mvcArray.push(new google.maps.LatLng(latlngNe.lat(), +180.00));  //NE
                 mvcArray.push(new google.maps.LatLng(latlngNw.lat(), nwLng));    //NW
                 mvcArray.push(new google.maps.LatLng(latlngSw.lat(), swLng));    //SW
                 mvcArray.push(new google.maps.LatLng(latlngSe.lat(), +180.00));  //SE

                 //Create new 'NorthWest-SouthWest' corners
                 latlngNw = new google.maps.LatLng(latlngNw.lat(), -180.00);
                 latlngSw = new google.maps.LatLng(latlngSw.lat(), -180.00);
             }

             //Check 'NorthEast-SouthEast' lng...
             var neLng = latlngNe.lng();
             var seLng = latlngSe.lng();
             if (0 >= neLng && 0 >= seLng) {
            //'NorthEast-SouthEast' points are in the 'west' (relative to Greenwich) 
            mvcArray = mvcArrays.west;

        mvcArray.push(new google.maps.LatLng(latlngNe.lat(), neLng));     //NE
                 mvcArray.push(new google.maps.LatLng(latlngNw.lat(), -180.0));    //NW
                 mvcArray.push(new google.maps.LatLng(latlngSw.lat(), -180.0));    //SW
                 mvcArray.push(new google.maps.LatLng(latlngSe.lat(), seLng));     //SE

                 //Create new 'NorthEast-SouthEast' corners
                 latlngNe = new google.maps.LatLng(latlngNw.lat(), +180.00);
                 latlngSe = new google.maps.LatLng(latlngSw.lat(), +180.00);
             }

             //ALWAYS value the center area...
             mvcArray = mvcArrays.center;

             mvcArray.push(latlngNe);    //NE
             mvcArray.push(latlngNw);    //NW
             mvcArray.push(latlngSw);    //SW
             mvcArray.push(latlngSe);    //SE

             for (var key in mvcArrays) {
            mvcArray = mvcArrays[key];
        var length = mvcArray.getLength();
                 if (0 < length) {
            console.log('********* ' + key + '************');
        console.log('NorthEast: ' + mvcArray.getAt(0).toString());
                     console.log('NorthWest: ' + mvcArray.getAt(1).toString());
                     console.log('SouthWest: ' + mvcArray.getAt(2).toString());
                     console.log('SouthEast: ' + mvcArray.getAt(3).toString());
                     console.log('**************************************');
                 }
             }
             //alert(mvcArrays[0])
             return mvcArrays;
         }
