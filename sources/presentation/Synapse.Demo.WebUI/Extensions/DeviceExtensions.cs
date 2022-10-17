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
    public static DeviceWidgetViewModel? AsViewModel(this Device? device, IMapper mapper)
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
                    var thermometer = mapper.Map<Thermometer>(device);
                    viewModel.IsActive = true;
                    var displayedTemperature = thermometer.DisplayedTemperature;
                    if (thermometer.DesiredTemperature != null)
                    {
                        displayedTemperature += "->" + thermometer.DisplayedDesiredTemperature;
                    }
                    viewModel.Data = displayedTemperature;
                    if (thermometer.Temperature.HasValue)
                    {
                        viewModel.Hero = new KnobHeroViewModel(0, 50, thermometer.Temperature.Value, "thermometer");
                    }
                    break;
                }
            case "sensor.hydrometer":
                {
                    var hydrometer = mapper.Map<Hydrometer>(device);
                    viewModel.IsActive = true;
                    viewModel.Data = hydrometer.DisplayedHumidity;
                    if (hydrometer.Humidity.HasValue)
                    {
                        viewModel.Hero = new KnobHeroViewModel(0, 100, hydrometer.Humidity.Value, "humidity_low");
                    }
                    break;
                }
            case "sensor.motion":
                {
                    var switchable = mapper.Map<Switchable>(device);
                    if (switchable.IsTurnedOn)
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
                    var switchable = mapper.Map<Switchable>(device);
                    if (switchable.IsTurnedOn)
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
                    var switchable = mapper.Map<Switchable>(device);
                    if (switchable.IsTurnedOn)
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
                    var switchable = mapper.Map<Switchable>(device);
                    if (switchable.IsTurnedOn)
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
