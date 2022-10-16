using System.Text.RegularExpressions;

namespace Synapse.Demo.WebUI.Extensions;

/// <summary>
/// Extension methods for <see cref="Device"/>s
/// </summary>
public static class DeviceExtensions
{
    /// <summary>
    /// Returns a <see cref="DeviceWidgetViewModel"/> for the targeted <see cref="Device"/>
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    public static DeviceWidgetViewModel? AsViewModel(this Device? device)
    {
        if (device == null) return null;
        DeviceWidgetViewModel viewModel = new DeviceWidgetViewModel()
        {
            Id = device.Id,
            Label = device.Label
        };
        switch (device.Type)
        {
            case "sensor.thermometer":
                {
                    viewModel.IsActive = true;
                    var displayedTemperature = device.State != null ? ((dynamic)device.State).temperature ?? "N/A" : "N/A";
                    if (device.State != null && ((dynamic)device.State).desired != null)
                    {
                        displayedTemperature += "->" + ((dynamic)device.State!).desired;
                    }
                    viewModel.Data = displayedTemperature;
                    var temperature = Regex.Match(viewModel.Data, @"\d+").Value;
                    if (!string.IsNullOrWhiteSpace(temperature) && int.TryParse(temperature, out int temperatureValue))
                    {
                        viewModel.Hero = new KnobHeroViewModel(0, 50, temperatureValue, "thermometer");
                    }
                    break;
                }
            case "sensor.hydrometer":
                {
                    viewModel.IsActive = true;
                    viewModel.Data = device.State != null ? ((dynamic)device.State).humidity ?? "N/A" : "N/A";
                    var humidity = Regex.Match(viewModel.Data, @"\d+").Value;
                    if (!string.IsNullOrWhiteSpace(humidity) && int.TryParse(humidity, out int humidityValue))
                    {
                        viewModel.Hero = new KnobHeroViewModel(0, 100, humidityValue, "humidity_low");
                    }
                    break;
                }
            case "sensor.motion":
                {
                    if (device.State != null && ((dynamic)device.State).on == true)
                    {
                        viewModel.Hero = "motion_sensor_active";
                        viewModel.Data = "-ON-";
                        viewModel.IsActive = true;
                    }
                    else
                    {
                        viewModel.Hero = "motion_sensor_idle";
                        viewModel.Data = "-OFF-";
                        viewModel.IsActive = false;
                    }
                    break;
                }
            case "switch.light":
                {
                    viewModel.Hero = "light";
                    if (device.State != null && ((dynamic)device.State).on == true)
                    { 
                        viewModel.Data = "-ON-";
                        viewModel.IsActive = true;
                    }
                    else
                    {
                        viewModel.Data = "-OFF-";
                        viewModel.IsActive = false;
                    }
                    break;
                }
            case "equipment.heater":
                {
                    viewModel.Hero = "fireplace";
                    if (device.State != null && ((dynamic)device.State).on == true)
                    {
                        viewModel.Data = "-ON-";
                        viewModel.IsActive = true;
                    }
                    else
                    {
                        viewModel.Data = "-OFF-";
                        viewModel.IsActive = false;
                    }
                    break;
                }
            case "equipment.air-conditioning":
                {
                    if (device.State != null && ((dynamic)device.State).on == true)
                    {
                        viewModel.Hero = "mode_cool";
                        viewModel.Data = "-ON-";
                        viewModel.IsActive = true;
                    }
                    else
                    {
                        viewModel.Hero = "mode_cool_off";
                        viewModel.Data = "-OFF-";
                        viewModel.IsActive = false;
                    }
                    break;
                }
        }
        return viewModel;
    }
}
