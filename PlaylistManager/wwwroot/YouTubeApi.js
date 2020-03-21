//var tag = document.createElement('script');
//tag.id = 'iframe-demo';
//tag.src = 'https://www.youtube.com/iframe_api';
//var firstScriptTag = document.getElementsByTagName('script')[0];
//firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
//var player;
//function onYouTubeIframeAPIReady() {
//    player = new YT.Player('player', {
//        playerVars: { 'autoplay': 1, 'controls': 0 },
//        events: {
//            'onReady': onPlayerReady,
//            'onStateChange': onPlayerStateChange,

//        }
//    });
//    console.log("Api ready");
//    console.log(logstring);
//}
window.removedItem = (number) => {
    var prefnum = number;
    console.log(prefnum);
    var item = document.querySelector('#ind-' + prefnum);
    item.parentNode.removeChild(item);
}
window.removeYouTube = () => {
    var elem = document.querySelector('#player');
    elem.parentNode.removeChild(elem);
}
window.addPlayer = () => {
    var maker = document.getElementById("makePlayer");
    var made = document.createElement("div");
    made.id = "player";
    maker.appendChild(made);
}
window.startYouTube = () => {
    var tag = document.createElement('script');
    tag.id = 'iframe-demo';
    tag.src = 'https://www.youtube.com/iframe_api';
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
}
window.getYouTube = (instance, video) => {
    if (typeof(YT) == 'undefined' || typeof(YT.Player) == 'undefined') {
        var tag = document.createElement('script');
        tag.id = 'iframe-demo';
        tag.src = 'https://www.youtube.com/iframe_api';
        var firstScriptTag = document.getElementsByTagName('script')[0];
        firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
        onYouTubeIframeAPIReady();
    } else {
         onYouTubeIframeAPIReady();
    }
    
    var player;
    function onYouTubeIframeAPIReady() {
        
        player = new YT.Player('player', {
            playerVars: { 'autoplay': 1, 'controls': 1 },
            videoId: video,
            height: 400,
            width: 600,            
            events: {
                'onReady': onPlayerReady,
                'onStateChange': onPlayerStateChange
            },
                        
        });
        console.log("Api ready");        
    }
    function onPlayerStateChange(event) {
        if (event.data === 0 | YT.PlayerState.ENDED) {
            console.log("Video ended");
            instance.invokeMethodAsync('GetNextVideo');           
            
        };
        console.log("state changed");
    }
   
    function onPlayerReady(event) {        
        event.target.playVideo();
        console.log("player ready");
    }

    //window.startApi = function () {
    //    var v = document.getElementById("player");

    //    v.addEventListener("ended", function () {
    //        DotNet.invokeMethodAsync("PlaylistManager", "PlayNext");
    //        console.log("event listner triggered");
    //    });
    //}
}