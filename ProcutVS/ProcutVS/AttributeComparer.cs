using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ProcutVS
{
	public class AttributeComparer
	{
		public static Dictionary<string, AttributeVSResult[]> SmartCompare(AttributePair[] pairs)
		{
			Dictionary<string, AttributeVSResult[]> vsResultDic = new Dictionary<string, AttributeVSResult[]>();
			foreach (var attributePair in pairs)
			{
				if (attributePair.Value1.Equals(attributePair.Value2))
					continue;
				if (!SmartVSValue.IsComparable(attributePair.Value1, attributePair.Value2))
				{
					if(attributePair.Value1.Equals(attributePair.Value2))
						continue;

					new NoneAttributeRule().SmartCompare(attributePair, vsResultDic);
					continue;
				}

				AttributeRule.GetRule(attributePair.Name).SmartCompare(attributePair, vsResultDic);
			}
			return vsResultDic;
		}
	}


	public class AttributePair
	{
		public string Name;
		public SmartVSValue Value1;
		public SmartVSValue Value2;
	}

	abstract class AttributeRule
	{
		public static AttributeRule GetRule(string name)
		{
			switch (name.ToLower())
			{
				case "best buy price":
				case "amazon price":
				case "amazon sale rank":
				case "best buy short term sale rank":
				case "best buy long term sale rank":
				case "best buy medium term sale rank":
					return new LowerBetterAttributeRule();

				case "processor speed":
					return new FasterBetterAttributeRule();

				case "warranty terms - parts":
				case "warranty terms - labor":
				case "length of cord":
				case "cordless shave time":
				case "standby time":
				case "talk time":
					return new LongerBetterAttributeRule();

				case "screen size (measured diagonally)":
				case "screen size":
				case "system memory (ram)":
				case "system memory (ram) expandable to":
				case "computer hard drive size":
				case "hard drive size":
				case "drive capacity":
				case "internal memory":
				case "megapixels (effective)":
				case "megapixels (total)":
				case "lcd screen size":
				case "digital magnification":
				case "cache memory":
				case "built-in storage capacity":
				case "capacity (mb/gb)":
				case "memory":
				case "number of fan speeds":
				case "max number of cds":
				case "max number of dvds":
				case "capacity":
				case "number of speed dials":
				case "capacity (cu. ft.)":
				case "bit depth":
				case "lines of resolution":
				case "resolution":
				case "wireless capability":
				case "rms power range (watts)":
				case "peak power watts per channel (2 ohms)":
				case "range":
				case "camera resolution":
				case "band and mode":
				case "horsepower motor":
				case "Capacity: Freezer (cu. ft.)":
				case "Capacity: Fresh-Food (cu. ft.)":
					return new BiggerBetterAttributeRule();

				case "bluetooth-enabled":
				case "hdmi output":
				case "image stitching":
				case "image stabilization":
				case "face detection":
				case "burst mode":
				case "panorama mode":
				case "movie mode":
				case "audio":
				case "hd movie mode":
				case "smile mode":
				case "waterproof":
				case "coldproof":
				case "shockproof":
				case "ac adapter":
				case "direct-disc labeling":
				case "blu-ray player":
				case "energy star qualified":
				case "bby software installer":
				case "best buy pc app":
				case "wireless display":
				case "multiroom capability":
				case "dolby digital 6.1 input":
				case "jpeg viewer":
				case "dvd player":
				case "wireless subwoofer":
				case "internet connectable":
				case "wireless rear speakers":
				case "usb input":
				case "3d-ready":
				case "wi-fi built in":
				case "wi-fi ready":
				case "ethernet port":
				case "smart capable":
				case "digital audio format upgradable":
				case "touch screen":
				case "wifi enabled":
				case "Plays movies":
				case "wireless networking":
				case "two-sided printer":
				case "edge cleaning":
				case "self propelled":
				case "bagless technology":
				case "cd text":
				case "ipod-ready":
				case "remote control":
				case "eq presets":
				case "mp3-playback capability":
				case "hd-ready":
				case "color display":
				case "subwoofer crossover":
				case "subwoofer output":
				case "customizable eq":
				case "built-in hd radio":
				case "cd title memory":
				case "changer controls":
				case "auto-reverse cassette":
				case "full logic cassette controls":
				case "noise reduction":
				case "radio data system":
				case "mp3 playback":
				case "bluetooth built-in":
				case "text to speech":
				case "waas-enabled":
				case "download capability":
				case "built-in base maps":
				case "pc connectivity":
				case "voice prompts":
				case "backlit display":
				case "water-resistant":
				case "battery included":
				case "dc adapter":
				case "hand strap":
				case "built-in 2-way radio":
				case "3d map view":
				case "accepts data cards":
				case "world travel clock":
				case "compass":
				case "pc-free printing":
				case "automatic redial":
				case "amp volume control":
				case "amp gain control":
				case "amp bass/mid/high control":
				case "stackable":
				case "automatic temperature control":
				case "delayed start":
				case "reversible door hinge":
				case "permanent press cycle":
				case "delicate cycle":
				case "hand-wash cycle":
				case "second rinse":
				case "bleach dispenser":
				case "fabric dispenser":
				case "pre-wash dispenser":
				case "water efficiency":
				case "compact design":
				case "steam":
				case "vibration reduction":
				case "high efficiency":
				case "sanitation/allergy cycle":
				case "depthfinder":
				case "fishfinder":
				case "sun/moon information":
				case "chartplotter":
				case "safety menu":
				case "programmable routes":
				case "programmable waypoints":
				case "internet/email capable":
				case "built-in digital camera":
				case "built-in gps":
				case "qwerty keyboard":
				case "graphics":
				case "vibrate mode":
				case "customizable ring tones":
				case "voice activated":
				case "downloadable games":
				case "speed dialing":
				case "mms":
				case "text messaging/instant messaging":
				case "vibration alert":
				case "speakerphone":
				case "downloadable ringtones":
				case "changeable faceplate":
				case "external caller id":
				case "dishwasher safe":
				case "pulse":
				case "special order":
				case "file storage":
				case "cool touch exterior":
				case "cool touch":
				case "crumb tray":
				case "heart rate monitor":
				case "fan":
				case "ifit interactive":
				case "motorized":
				case "ipod compatible":
				case "mp3 compatible":
				case "speakers":
				case "adjustable incline":
				case "headphone jack":
				case "touch pad controls":
				case "extra-delicate cycle":
				case "led buttons":
				case "lint filter light":
				case "moisture sensor":
				case "end-of-cycle signal":
				case "interior light":
				case "custom settings":
				case "drying rack":
				case "plug and play":
				case "hot swappable":
				case "mac compatible":
				case "counter depth":
				case "thru-the-door ice dispenser":
				case "thru-the-door water dispenser":
				case "factory-installed icemaker":
				case "gallon door storage":
				case "humidity-controlled crisper":
				case "child lock":
				case "door-open alarm":
				case "filter light reminder":
				case "external drain connector":
				case "timer":
				case "moisture control":
				case "humidistat humidity control":
				case "manual dryout option":
				case "thermostat heat control":
				case "overload protection":
				case "auto safety shutoff":
				case "tip over shutoff":
				case "humidity control":
				case "pop-up trimmer":
				case "battery level indicator":
				case "battery indicator":
				case "wet/dry":
				case "editing features":
				case "motion sensor":
				case "audio sensor":
				case "video light":
				case "manual focus":
				case "auto date/time stamp":
				case "preset titles":
				case "programmed recording":
				case "color viewfinder":
				case "custom titling":
				case "line input recording":
				case "time lapse recording":
				case "rechargeable battery":
				case "playback adapter":
				case "shoot and share":
				case "high definition":
				case "vista compatible":
				case "sabbath mode":
				case "simmer burner":
				case "fan cooled":
				case "low-pass filter":
				case "passthrough":
				case "1 ohm stable":
				case "tri-mode":
				case "line-level input":
				case "speaker-level input":
				case "bass boost":
				case "bridgeable":
				case "sealed burners":
				case "electronic ignition":
				case "self-cleaning":
				case "convection":
				case "oven window":
				case "oven light":
				case "double oven":
				case "water filtration":
				case "car security":
				case "keyless entry":
				case "remote start":
				case "1 way":
				case "2 way":
				case "led":
				case "lcd":
				case "force feedback":
				case "\"tilt\" technology":
				case "\"rumble\" technology":
				case "throttle control":
				case "gaming series":
				case "cordless/wireless":
				case "mouse included":
				case "palm rest":
				case "hdmi input":
				case "speakers included":
				case "tilt":
				case "dvi-d with hdcp input":
				case "on-screen image controls":
				case "antiglare coating":
				case "vga input":
				case "dvi-d input":
				case "touchscreen":
				case "centrifugal switch control":
					return new YesBetterAttributeRule();

				case "usb 2.0 ports":
				case "s-video outputs":
				case "pcmcia slots":
				case "dvi inputs":
				case "hdmi inputs":
				case "component video inputs":
				case "component video outputs":
				case "hdmi outputs":
				case "dvi outputs":
				case "digital audio inputs":
				case "analog audio input":
				case "battery quantity":
				case "cycles (number of)":
				case "wash cycles":
				case "total print heads":
				case "number of ink colors":
				case "number of cartridges":
				case "expansion slots":
				case "cooling fans":
				case "number of ccds":
				case "racks (number of)":
				case "number of channels":
				case "number of doors":
				case "number of buttons":
				case "number of programmable buttons":
					return new MoreBetterAttributeRule();

				case "product width":
				case "product height":
				case "product depth":
				case "imaging sensor size":
				case "total system power":
				case "subwoofer power (watts)":
				case "subwoofer power":
				case "number of speakers":
				case "tuning presets":
				case "btus":
				case "voltage":
				case "cleaning path width":
				case "voltage of pre-outs":
				case "number of pre-outs":
				case "amp power (watts)":
				case "amp battery size":
				case "nut width":
				case "scale length":
				case "fingerboard radius":
				case "depth with door open (in.)":
				case "amp power rms (watts)":
				case "transfer rate":
				case "total capacity (cu. ft.)":
				case "sensitivity":
				case "depth including handles (in.)":
				case "depth less door (in.)":
				case "depth with door 90° open (in.)":
				case "depth without handles (in.)":
					return new BiggerAttributeRule();

				case "number of frets":
				case "amp battery quantity":
				case "shelves":
				case "number of slices":
				case "number of waypoints":
					return new MoreAttributeRule();

				case "product weight":
				case "maximum weight (lbs.)":
					return new HeavierAttributeRule();

				case "digital zoom":
				case "optical zoom":
					return new HigherBetterAttributeRule();

				case "energy consumption (kwh/year)":
				case "recharge time":
				case "response time":
				case "estimated yearly operating cost":
					return new LessBetterAttributeRule();

				default:
					return new NoneAttributeRule();
			}
		}

		public virtual void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = "",
			                           				GoodBad = GoodBad.Normal,
			                           				Value = pair.Value1.RawValue,
			                           				Diff = ""
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = "",
			                           				GoodBad = GoodBad.Normal,
			                           				Value = pair.Value2.RawValue,
			                           				Diff = ""
			                           			}
			                           	});
		}
	}


	class NoneAttributeRule : AttributeRule
	{
	}

	class BiggerAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ? "Bigger" : "Smaller",
			                           				GoodBad = GoodBad.Normal,
													Value=pair.Value1.RawValue,
			                           				Diff = ((pair.Value1.Value > pair.Value2.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ? "Bigger" : "Smaller",
			                           				GoodBad = GoodBad.Normal,
													Value=pair.Value2.RawValue,
			                           				Diff = ((pair.Value2.Value > pair.Value1.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			}
			                           	});
		}
	}
	class HeavierAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ? "Heavier" : "Lighter",
			                           				GoodBad = GoodBad.Normal,
													Value=pair.Value1.RawValue,
			                           				Diff = ((pair.Value1.Value > pair.Value2.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ? "Heavier" : "Lighter",
			                           				GoodBad = GoodBad.Normal,
													Value=pair.Value2.RawValue,
			                           				Diff = ((pair.Value2.Value > pair.Value1.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			}
			                           	});
		}
	}

	class FasterAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ? "Faster" : "Slower",
			                           				GoodBad = GoodBad.Normal,
													Value=pair.Value1.RawValue,
			                           				Diff = ((pair.Value1.Value > pair.Value2.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ? "Faster" : "Slower",
			                           				GoodBad = GoodBad.Normal,
													Value=pair.Value2.RawValue,
			                           				Diff = ((pair.Value2.Value > pair.Value1.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			}
			                           	});
		}
	}

	class BiggerBetterAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ? "Bigger" : "Smaller",
			                           				GoodBad = pair.Value1.Value > pair.Value2.Value ? GoodBad.Good : GoodBad.Bad,
													Value=pair.Value1.RawValue,
			                           				Diff = ((pair.Value1.Value > pair.Value2.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ? "Bigger" : "Smaller",
			                           				GoodBad = pair.Value2.Value > pair.Value1.Value ? GoodBad.Good : GoodBad.Bad,
													Value=pair.Value2.RawValue,
			                           				Diff = ((pair.Value2.Value > pair.Value1.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			}
			                           	});
		}
	}

	class HigherBetterAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ? "Higher" : "Lower",
			                           				GoodBad = pair.Value1.Value > pair.Value2.Value ? GoodBad.Good : GoodBad.Bad,
													Value=pair.Value1.RawValue,
			                           				Diff = ((pair.Value1.Value > pair.Value2.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ? "Higher" : "Lower",
			                           				GoodBad = pair.Value2.Value > pair.Value1.Value ? GoodBad.Good : GoodBad.Bad,
													Value=pair.Value2.RawValue,
			                           				Diff = ((pair.Value2.Value > pair.Value1.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			}
			                           	});
		}
	}

	class FasterBetterAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ? "Faster" : "Slower",
			                           				GoodBad = pair.Value1.Value > pair.Value2.Value ? GoodBad.Good : GoodBad.Bad,
													Value=pair.Value1.RawValue,
			                           				Diff = ((pair.Value1.Value > pair.Value2.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ? "Faster" : "Slower",
			                           				GoodBad = pair.Value2.Value > pair.Value1.Value ? GoodBad.Good : GoodBad.Bad,
													Value=pair.Value2.RawValue,
			                           				Diff = ((pair.Value2.Value > pair.Value1.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			}
			                           	});
		}
	}

	class LongerBetterAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ? "Longer" : "Shorter",
			                           				GoodBad = pair.Value1.Value > pair.Value2.Value ? GoodBad.Good : GoodBad.Bad,
													Value=pair.Value1.RawValue,
			                           				Diff = ((pair.Value1.Value > pair.Value2.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ? "Longer" : "Shorter",
			                           				GoodBad = pair.Value2.Value > pair.Value1.Value ? GoodBad.Good : GoodBad.Bad,
													Value=pair.Value2.RawValue,
			                           				Diff = ((pair.Value2.Value > pair.Value1.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			}
			                           	});
		}
	}

	class LongerAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ? "Longer" : "Shorter",
			                           				GoodBad = GoodBad.Normal,
													Value=pair.Value1.RawValue,
			                           				Diff = ((pair.Value1.Value > pair.Value2.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ? "Longer" : "Shorter",
			                           				GoodBad = GoodBad.Normal,
													Value=pair.Value2.RawValue,
			                           				Diff = ((pair.Value2.Value > pair.Value1.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			}
			                           	});
		}
	}
	class MoreBetterAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ? "More" : "Less",
			                           				GoodBad = pair.Value1.Value > pair.Value2.Value ? GoodBad.Good : GoodBad.Bad,
													Value=pair.Value1.RawValue,
			                           				Diff = ((pair.Value1.Value > pair.Value2.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ? "More" : "Less",
			                           				GoodBad = pair.Value2.Value > pair.Value1.Value ? GoodBad.Good : GoodBad.Bad,
													Value=pair.Value2.RawValue,
			                           				Diff = ((pair.Value2.Value > pair.Value1.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			}
			                           	});
		}
	}

	class MoreAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ? "More" : "Less",
			                           				GoodBad = GoodBad.Normal,
													Value=pair.Value1.RawValue,
			                           				Diff = ((pair.Value1.Value > pair.Value2.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ? "More" : "Less",
			                           				GoodBad = GoodBad.Normal,
													Value=pair.Value2.RawValue,
			                           				Diff = ((pair.Value2.Value > pair.Value1.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			}
			                           	});
		}
	}

	class LowerBetterAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ? "Higher" : "Lower",
			                           				GoodBad = pair.Value1.Value > pair.Value2.Value ? GoodBad.Bad : GoodBad.Good,
													Value=pair.Value1.RawValue,
			                           				Diff = ((pair.Value1.Value > pair.Value2.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ? "Higher" : "Lower",
			                           				GoodBad = pair.Value2.Value > pair.Value1.Value ? GoodBad.Bad : GoodBad.Good,
													Value=pair.Value2.RawValue,
			                           				Diff = ((pair.Value2.Value > pair.Value1.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
												}
			                           	});
		}
	}

	class LessBetterAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ? "Bigger" : "Smaller",
			                           				GoodBad = pair.Value1.Value > pair.Value2.Value ? GoodBad.Bad : GoodBad.Good,
													Value=pair.Value1.RawValue,
			                           				Diff = ((pair.Value1.Value > pair.Value2.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ? "Bigger" : "Smaller",
			                           				GoodBad = pair.Value2.Value > pair.Value1.Value ? GoodBad.Bad : GoodBad.Good,
													Value=pair.Value2.RawValue,
			                           				Diff = ((pair.Value2.Value > pair.Value1.Value)?"+":"-") + Math.Abs(pair.Value1.Value - pair.Value2.Value)+pair.Value1.Unit
												}
			                           	});
		}
	}

	class YesBetterAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			if (!"yes,no,none".Contains(pair.Value1.RawValue.ToLower())
				|| !"yes,no,none".Contains(pair.Value2.RawValue.ToLower()))
			{
				base.SmartCompare(pair, vsResultDic);
				return;
			}

			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.RawValue,
			                           				GoodBad = pair.Value1.RawValue.ToLower() == "no" ? GoodBad.Bad : GoodBad.Good,
			                           				Value = "",
			                           				Diff = ""
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.RawValue,
			                           				GoodBad = pair.Value2.RawValue.ToLower() == "no" ? GoodBad.Bad : GoodBad.Good,
			                           				Value = "",
			                           				Diff = ""
			                           			}
			                           	});
		}
	}

	class RankAttributeRule : AttributeRule
	{
		public override void SmartCompare(AttributePair pair, Dictionary<string, AttributeVSResult[]> vsResultDic)
		{
			vsResultDic.Add(pair.Name, new AttributeVSResult[]
			                           	{
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value1.Value > pair.Value2.Value ?  "Lower":"Higher" ,
			                           				GoodBad = pair.Value1.Value > pair.Value2.Value ? GoodBad.Good: GoodBad.Bad,
													Value=pair.Value1.RawValue,
			                           				Diff = ""
			                           			},
			                           		new AttributeVSResult()
			                           			{
			                           				Name = pair.Name,
			                           				Description = pair.Value2.Value > pair.Value1.Value ?  "Lower":"Higher" ,
			                           				GoodBad = pair.Value2.Value > pair.Value1.Value ? GoodBad.Good:GoodBad.Bad ,
													Value=pair.Value2.RawValue,
			                           				Diff = ""
												}
			                           	});
		}
	}


	public class AttributeVSResult : IComparable<AttributeVSResult>
	{
		public string Name;
		public string Description;
		public string Diff;
		public string Value;
		public GoodBad GoodBad;

		public override string ToString()
		{
			return string.Format("Name: {0}, Value:{4} Desc: {1}, Diff: {2}, GoodBad: {3}",
								 Name, Description, Diff, GoodBad, Value);
		}

		#region IComparable<AttributeVSResult> Members

		public int CompareTo(AttributeVSResult other)
		{
			if (GoodBad != other.GoodBad)
				return GoodBad.CompareTo(other.GoodBad);

			return Name.CompareTo(other.Name);
		}

		#endregion
	}

	public enum GoodBad
	{
		Good,
		Bad,
		Normal,
	}

	public class SmartVSValue
	{
		static readonly Regex numberReg = new Regex(@"^([\d\.]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
		static readonly Regex uselessReg = new Regex(@"up to\s*", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

		public decimal Value;
		public string RawValue;
		public string Unit = "";
		public bool PreposeUnit;

		public SmartVSValue()
		{
			PreposeUnit = false;
		}

		public SmartVSValue(string value)
			: this()
		{
			RawValue = value;
			value = uselessReg.Replace(value, "");
			Match match = numberReg.Match(value);
			if (decimal.TryParse(match.Groups[0].Value, out Value))
			{
				Unit = value.Substring(match.Groups[0].Index + match.Groups[0].Length);
			}
		}

		public override bool Equals(object obj)
		{
			SmartVSValue v2 = (SmartVSValue)obj;

			return Value == v2.Value
				   && Unit == v2.Unit
				   && RawValue == v2.RawValue;
		}

		internal static bool IsComparable(SmartVSValue smartVSValue1, SmartVSValue smartVSValue2)
		{
			if (smartVSValue1.Unit == smartVSValue2.Unit)
				return true;

			if (UnitConverter.TryConvert(smartVSValue1, smartVSValue2))
				return true;

			return false;
		}

	}


	/**/
	class UnitConverter
	{
		static Dictionary<string, decimal> WeightUnitDic = new Dictionary<string, decimal>();
		static Dictionary<string, decimal> ByteUnitDic = new Dictionary<string, decimal>();
		static Dictionary<string, decimal> DateSpanUnitDic = new Dictionary<string, decimal>();
		static Dictionary<string, decimal> TimeSpanUnitDic = new Dictionary<string, decimal>();

		static UnitConverter()
		{
			ByteUnitDic["K"] = 1;
			ByteUnitDic["M"] = 1024;
			ByteUnitDic["G"] = 1024 * 1024;
			ByteUnitDic["T"] = 1024 * 1024 * 1024;

			ByteUnitDic["KB"] = 1;
			ByteUnitDic["MB"] = 1024;
			ByteUnitDic["GB"] = 1024 * 1024;
			ByteUnitDic["TB"] = 1024 * 1024 * 1024;
			//
			DateSpanUnitDic["YEAR"] = 365;
			DateSpanUnitDic["YEARS"] = 365;
			DateSpanUnitDic["YEAR LIMITED"] = 365;
			DateSpanUnitDic["YEARS LIMITED"] = 365;
			DateSpanUnitDic["MONTH"] = 30;
			DateSpanUnitDic["MONTHS"] = 30;
			DateSpanUnitDic["MONTH LIMITED"] = 30;
			DateSpanUnitDic["MONTHS LIMITED"] = 30;
			DateSpanUnitDic["DAY"] = 1;
			DateSpanUnitDic["DAYS"] = 1;
			DateSpanUnitDic["DAY LIMITED"] = 1;
			DateSpanUnitDic["DAYS LIMITED"] = 1;

			//
			TimeSpanUnitDic["DAY"] = 3600*24;
			TimeSpanUnitDic["DAYS"] = 3600*24;
			TimeSpanUnitDic["HOURS"] = 3600;
			TimeSpanUnitDic["HOUR"] = 3600;
			TimeSpanUnitDic["MINUSTES"] = 60;
			TimeSpanUnitDic["MINUSTE"] = 60;
			TimeSpanUnitDic["SECONDS"] = 1;
			TimeSpanUnitDic["SECOND"] = 1;
		}

		static decimal Convert(string from, string to)
		{
			if (ByteUnitDic.ContainsKey(from)
				&& ByteUnitDic.ContainsKey(to))
				return ByteUnitDic[from] / ByteUnitDic[to];
			if (WeightUnitDic.ContainsKey(from)
				&& WeightUnitDic.ContainsKey(to))
				return WeightUnitDic[from] / WeightUnitDic[to];
			if (DateSpanUnitDic.ContainsKey(from)
				&& DateSpanUnitDic.ContainsKey(to))
				return DateSpanUnitDic[from] / DateSpanUnitDic[to];
			if (TimeSpanUnitDic.ContainsKey(from)
				&& TimeSpanUnitDic.ContainsKey(to))
				return TimeSpanUnitDic[from] / TimeSpanUnitDic[to];

			return 1;
		}

		internal static bool TryConvert(SmartVSValue v1, SmartVSValue v2)
		{
			decimal x = 0;
			string unit1 = v1.Unit.ToUpper().Trim();
			string unit2 = v2.Unit.ToUpper().Trim();
			if (ByteUnitDic.ContainsKey(unit1)
			&& ByteUnitDic.ContainsKey(unit2))
			{
				x = ByteUnitDic[unit1] / ByteUnitDic[unit2];
			}
			if (WeightUnitDic.ContainsKey(unit1)
				&& WeightUnitDic.ContainsKey(unit2))
			{
				x = WeightUnitDic[unit1] / WeightUnitDic[unit2];
			}
			if (DateSpanUnitDic.ContainsKey(unit1)
				&& DateSpanUnitDic.ContainsKey(unit2))
			{
				x = DateSpanUnitDic[unit1] / DateSpanUnitDic[unit2];
			}
			if (TimeSpanUnitDic.ContainsKey(unit1)
				&& TimeSpanUnitDic.ContainsKey(unit2))
			{
				x = TimeSpanUnitDic[unit1] / TimeSpanUnitDic[unit2];
			}

			if (x != 0)
			{
				v1.Value = ((int)(v1.Value * x * 100)) / (decimal)100;
				v1.Unit = v2.Unit;
				return true;
			}

			return false;
		}
	}
}