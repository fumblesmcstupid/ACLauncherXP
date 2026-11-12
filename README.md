<h2>Launcher/server browser for Asheron's Call.<br>Made for old PC's that can't run ThwargLauncher.</h2>
<img width="666" height="503" alt="Screenshot_20261112_001618" src="https://github.com/user-attachments/assets/45b97633-5ed1-4596-983e-14eb8cf56e9f" />

<h3>Written and tested on Windows XP SP3 using Visual Studio 2010. Requires .NET Framework 4.0!</h3>

Stores settings, username, password and server to the registry at <code>HKEY_CURRENT_USER\Software\ACLauncherXP</code>.<br>
Only saves login when "Remember" is checked.

<h3>To play</h3>
<ul>
  <li>Install the game and end-of-retail client files: https://emulator.ac/how-to-play/</li>
  <li>Download and run <code>ACLauncherXP.exe</code> from Releases or compile it in Visual Studio.</li>
</ul>

<h3>To compile</h3>
Currently uses curl 7.80 to fetch the server list. Not included.<br>
Grab it here: https://www.npackd.org/p/se.haxx.curl.CURL/7.80<br>
Put <code>curl.exe</code> in the "Resources" folder to compile successfully.

<h3>Future plans</h3>
<ul>
  <li>Remove curl requirement.</li>
  <li>Allow user to disable <code>acclient.exe</code> SHA256 hash check. Currently requires the End-of-retail EXE.</li>
  <li>Allow sorting servers.</li>
  <li>Save multiple accounts.</li>
</ul>
