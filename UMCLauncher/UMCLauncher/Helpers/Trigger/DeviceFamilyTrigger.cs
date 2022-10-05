using Windows.System.Profile;
using Microsoft.UI.Xaml;

namespace UMCLauncher.Helpers.Trigger
{
    // https://docs.microsoft.com/windows/apps/design/devices/designing-for-tv#custom-visual-state-trigger-for-xbox
    public class DeviceFamilyTrigger : StateTriggerBase
    {
        private bool _isNegation = false;
        private string _actualDeviceFamily;
        private string _triggerDeviceFamily;

        public string DeviceFamily
        {
            get => _triggerDeviceFamily;
            set
            {
                if (_triggerDeviceFamily != value)
                {
                    _triggerDeviceFamily = value;
                    PropertyChanged(_triggerDeviceFamily, _isNegation);
                }
            }
        }

        public bool IsNegation
        {
            get => _isNegation;
            set
            {
                if (_isNegation != value)
                {
                    _isNegation = value;
                    PropertyChanged(_triggerDeviceFamily, _isNegation);
                }
            }
        }

        private void PropertyChanged(string _triggerDeviceFamily, bool _isNegation)
        {
            _actualDeviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            bool istrue = _actualDeviceFamily == _triggerDeviceFamily;
            SetActive(_isNegation ? !istrue : istrue);
        }
    }
}