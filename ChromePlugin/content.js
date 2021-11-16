let timerId = setTimeout(function tick() {
	try
	{
		if(document.getElementById("error").textContent == "Температура")
		{
			var myAudio = new Audio(chrome.extension.getURL("test.mp3"));
			myAudio.play();
		}
	}
	catch (err)
	{
		;
	}
	timerId = setTimeout(tick, 3000);
}, 
3000);