var client = new ZeroClipboard(document.getElementById("copybutton")); 

client.on("ready", function (readyEvent) {
    // alert( "ZeroClipboard SWF is ready!" );

    client.on("aftercopy", function (event) {
        // `this` === `client`
        // `event.target` === the element that was clicked
        //event.target.style.display = "none";
        alert("URL Copied Successfully");
    });
});

// main.js
//var client = new ZeroClipboard(document.getElementById("copy-button"), {
//    moviePath: "zeroClipboard/ZeroClipboard.swf"
//});

//client.on("load", function (client) {
//     alert( "movie is loaded" );

//    client.on("complete", function (client, args) {
//         `this` is the element that was clicked
//        this.style.display = "none";
//        alert("Copied text to clipboard: " + args.text);
//    });
//});