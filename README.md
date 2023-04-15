# OBSAutoRunner
This program allows you to automatically control OBS<br>
You can:<br>
- Start and stop streams<br>
- Change profile<br>
- Change stream key<br>
- Change stream video<br>

Don't forget to fill in the settings.json file before using<br>
It should include:<br>
```json
{
  "corePath": "..\\obs-studio\\basic",
  "startScript": "start /d \"..\\obs-studio\\bin\\64bit\" obs64.exe --startstreaming",
  "closeScript": "taskkill /F /IM obs64.exe",
  "profiles": [
    {
      "name": "Just random name",
      "streamKey": "stream_key",
      "videoPath": "path_to_the_video"
    }
  ]
}
```
