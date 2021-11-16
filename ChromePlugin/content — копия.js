let timerId = setTimeout(function tick() {
	try
	{
		if(document.getElementById("free_play_form_button").style.display != "none")
		{
			document.getElementById('free_play_form_button').click();
		}
	}
	catch (err)
	{
		;
	}
	timerId = setTimeout(tick, 3000);
}, 
3000);