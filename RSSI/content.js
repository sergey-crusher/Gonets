let timerId = setTimeout(function tick() {
	try
	{
		var RSSI = document.getElementById("RSSI").textContent;
		RSSI = RSSI.split(" ");
		RSSI = RSSI[0] * (-1);
		if(RSSI < 105)
		{
			var myAudio = new Audio(chrome.extension.getURL("./alarm.mp3"));
			myAudio.play();
		}
	}
	catch (err)
	{
		;
	}
	timerId = setTimeout(tick, 5000);
}, 
5000);