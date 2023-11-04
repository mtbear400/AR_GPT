using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI; // Import this to access the Interactable component.

namespace OpenAI
{
    public class WhisperKeys : MonoBehaviour
    {
        [SerializeField] private Image progressBar;
        [SerializeField] private Text message;
        [SerializeField] private Dropdown dropdown;
        [SerializeField] private InputField inputFieldMessage;
        [SerializeField] private Text audioSizeText;

        [Header("Piano Keys")]
        [SerializeField] private Interactable startKeyC; // Drag the C key with Interactable component here.
        [SerializeField] private Interactable stopKeyB;  // Drag the B key with Interactable component here.

        private readonly string fileName = "output.wav";

        private AudioClip clip;
        private bool isRecording;
        private OpenAIApi openai = new OpenAIApi("");

        private void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            dropdown.options.Add(new Dropdown.OptionData("Microphone not supported on WebGL"));
#else
            foreach (var device in Microphone.devices)
            {
                dropdown.options.Add(new Dropdown.OptionData(device));
            }
            dropdown.onValueChanged.AddListener(ChangeMicrophone);

            var index = PlayerPrefs.GetInt("user-mic-device-index");
            dropdown.SetValueWithoutNotify(index);

            // Attach the interaction methods to the piano keys' events.
            //startKeyC.OnClick.AddListener(StartRecording);
            //stopKeyB.OnClick.AddListener(EndRecording);
#endif
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }

        public void StartRecording()
        {
            Debug.Log("Start Recording called.");
            if (isRecording) return; // Prevent restarting the recording if it's already in progress.

            isRecording = true;
            message.text = "Listening...";
            var index = PlayerPrefs.GetInt("user-mic-device-index");

#if !UNITY_WEBGL
            clip = Microphone.Start(dropdown.options[index].text, false, 20, 44100);            // Use int.MaxValue for "unlimited" time, but keep in mind, it's still limited.
            Debug.Log("Is Recording? " + Microphone.IsRecording(dropdown.options[index].text));

#endif
        }

        public async void EndRecording()
        {
            DebugConsole.Log("End Recording called.");
            if (!isRecording) return;
            message.text = "Transcripting...";
            if (Microphone.IsRecording(dropdown.options[PlayerPrefs.GetInt("user-mic-device-index")].text))
            {
                Microphone.End(dropdown.options[PlayerPrefs.GetInt("user-mic-device-index")].text);
            }

            if (clip != null)
            {
                DebugConsole.Log("Clip is not null.");
                clip = SaveWav.TrimSilence(clip, 0.01f);
                byte[] data = SaveWav.Save(fileName, clip);
                audioSizeText.text = "Audio Size: " + data.Length + " bytes";
                var req = new CreateAudioTranscriptionsRequest
                {
                    FileData = new FileData() { Data = data, Name = "audio.wav" },
                    Model = "whisper-1",
                    Language = "en"
                };
                var res = await openai.CreateAudioTranscription(req);
                progressBar.fillAmount = 0;
                message.text = res.Text;
                inputFieldMessage.text = res.Text;
                isRecording = false;
            }
            else
            {
                message.text = "Null clip";
            }
        }



        private void Update()
        {
            if (isRecording)
            {
                // We're keeping this here in case you'd like to add a visual progress indicator in the future.
                // time += Time.deltaTime;
                // progressBar.fillAmount = time / duration;
            }
        }
    }
}
