using System.Text.RegularExpressions;

namespace EmailReplyParser
{
	public static class RegexPatterns
	{
		public static readonly Regex[] QuoteHeadersRegex = {
			new Regex(@"^-*\s*(On\s.+\s.+\n?wrote:{0,1})\s{0,1}-*$", RegexOptions.Multiline | RegexOptions.Compiled), // On DATE, NAME <EMAIL> wrote:
			new Regex(@"^-*\s*(Le\s.+\s.+\n?écrit\s?:{0,1})\s{0,1}-*$", RegexOptions.Multiline | RegexOptions.Compiled), // Le DATE, NAME <EMAIL> a écrit :
			new Regex(@"^-*\s*(El\s.+\s.+\n?escribió:{0,1})\s{0,1}-*$", RegexOptions.Multiline | RegexOptions.Compiled), // El DATE, NAME <EMAIL> escribió:
			new Regex(@"^-*\s*(Il\s.+\s.+\n?scritto:{0,1})\s{0,1}-*$", RegexOptions.Multiline | RegexOptions.Compiled), // Il DATE, NAME <EMAIL> ha scritto:
			new Regex(@"^-*\s*(Em\s.+\s.+\n?escreveu:{0,1})\s{0,1}-*$", RegexOptions.Multiline | RegexOptions.Compiled), // Em DATE, NAME <EMAIL> ha escreveu:
			new Regex(@"^\s*(Am\s.+\s)\n?\n?schrieb.+\s?(\[|<).+(\]|>):$", RegexOptions.Multiline | RegexOptions.Compiled), // Am DATE schrieb NAME <EMAIL>:

			new Regex(@"^\s*(Op\s[\s\S]+?\n?schreef[\s\S]+:)$", RegexOptions.Multiline | RegexOptions.Compiled), // Op DATE, schreef NAME <EMAIL>:
			new Regex(@"^\s*((W\sdniu|Dnia)\s[\s\S]+?(pisze|napisał(\(a\))?):)$", RegexOptions.Multiline | RegexOptions.Compiled), // W dniu DATE, NAME <EMAIL> pisze|napisał:
			new Regex(@"^\s*(Den\s.+\s\n?skrev\s.+:)$", RegexOptions.Multiline | RegexOptions.Compiled), // Den DATE skrev NAME <EMAIL>:
			new Regex(@"^\s*(pe\s.+\s.+\n?kirjoitti:)$", RegexOptions.Multiline | RegexOptions.Compiled), // pe DATE NAME <EMAIL> kirjoitti:
			new Regex(@"^\s*(Am\s.+\sum\s.+\s\n?schrieb\s.+:)$", RegexOptions.Multiline | RegexOptions.Compiled), // Am DATE um TIME schrieb NAME:
			new Regex(@"^\s*(ср\,\s.+\n?\sг\. в\s.+,\s.+[\[|<].+[\]|>]:)$", RegexOptions.Multiline | RegexOptions.Compiled), // ср, DATE г. в TIME, NAME <EMAIL>:
			new Regex(@"^(在[\s\S]+写道：)$", RegexOptions.Multiline | RegexOptions.Compiled), // > 在 DATE, TIME, NAME 写道：
			new Regex(@"^(20[0-9]{2}\..+\s작성:)$", RegexOptions.Multiline | RegexOptions.Compiled), // DATE TIME NAME 작성:
			new Regex(@"^(20[0-9]{2}\/.+のメッセージ:)$", RegexOptions.Multiline | RegexOptions.Compiled), // DATE TIME、NAME のメッセージ:
			new Regex(@"^(.+\s<.+>\sschrieb:)$", RegexOptions.Multiline | RegexOptions.Compiled), // NAME <EMAIL> schrieb:
			new Regex(@"^(.+\son.*at.*wrote:)$", RegexOptions.Multiline | RegexOptions.Compiled), // NAME on DATE wrote:
			new Regex(
				@"^\s*(From\s?:.+\s?\n?\s*[\[|<].+[\]|>])", RegexOptions.Multiline | RegexOptions.Compiled), // "From: NAME <EMAIL>" OR "From : NAME <EMAIL>" OR "From : NAME<EMAIL>"(With support whitespace before start and before <)
			new Regex(
				@"^\s*(Von\s?:.+\s?\n?\s*[\[|<].+[\]|>])", RegexOptions.Multiline | RegexOptions.Compiled), // "Von: NAME <EMAIL>" OR "Von : NAME <EMAIL>" OR "Von : NAME<EMAIL>" (German From)
			new Regex(
				@"^\s*(De\s?:.+\s?\n?\s*(\[|<).+(\]|>))", RegexOptions.Multiline | RegexOptions.Compiled), // "De: NAME <EMAIL>" OR "De : NAME <EMAIL>" OR "De : NAME<EMAIL>"  (With support whitespace before start and before <)
			new Regex(
				@"^\s*(Van\s?:.+\s?\n?\s*(\[|<).+(\]|>))", RegexOptions.Multiline | RegexOptions.Compiled), // "Van: NAME <EMAIL>" OR "Van : NAME <EMAIL>" OR "Van : NAME<EMAIL>"  (With support whitespace before start and before <)
			new Regex(
				@"^\s*(Da\s?:.+\s?\n?\s*(\[|<).+(\]|>))", RegexOptions.Multiline | RegexOptions.Compiled), // "Da: NAME <EMAIL>" OR "Da : NAME <EMAIL>" OR "Da : NAME<EMAIL>"  (With support whitespace before start and before <)
			new Regex(
				@"^(20[0-9]{2})-([0-9]{2}).([0-9]{2}).([0-9]{2}):([0-9]{2})\n?(.*)>:$", RegexOptions.Multiline | RegexOptions.Compiled), // 20YY-MM-DD HH:II GMT+01:00 NAME <EMAIL>:
			new Regex(@"^\s*([a-z]{3,4}\.\s[\s\S]+\sskrev\s[\s\S]+:)$", RegexOptions.Multiline | RegexOptions.Compiled), // DATE skrev NAME <EMAIL>:
			new Regex(
				@"^([0-9]{2}).([0-9]{2}).(20[0-9]{2})(.*)(([0-9]{2}).([0-9]{2}))(.*)""( *)<(.*)>( *):$", RegexOptions.Multiline | RegexOptions.Compiled), // DD.MM.20YY HH:II NAME <EMAIL>
			new Regex(@"^[0-9]{2}:[0-9]{2}(.*)[0-9]{4}(.*)""( *)<(.*)>( *):", RegexOptions.Compiled), // HH:II, DATE, NAME <EMAIL>:
			new Regex(@"^(.*)[0-9]{4}(.*)from(.*)<(.*)>:", RegexOptions.Compiled), // DATE from NAME <EMAIL>:
			new Regex(@"^-{1,12} ?(O|o)riginal (M|m)essage ?-{1,12}$", RegexOptions.IgnoreCase | RegexOptions.Compiled), // --- Original Message ---
			new Regex(@"^-{1,12} ?(O|o)prindelig (B|b)esked ?-{1,12}$", RegexOptions.IgnoreCase | RegexOptions.Compiled), // --- Oprindelig Besked --- (DA)
			new Regex(@"^-{1,12} ?(M|m)essage d'origine ?-{1,12}$", RegexOptions.IgnoreCase | RegexOptions.Compiled), // --- Message d'origine --- (FR)
			new Regex(@"^-{1,12} ?(U|u)rsprüngliche (N|n)achricht ?-{0,12}$", RegexOptions.IgnoreCase | RegexOptions.Compiled), // --- Ursprüngliche Nachricht --- (DE)
		};

		public static readonly Regex[] SignatureRegex = {
			new Regex(@"^\s*-{2,4}$", RegexOptions.Compiled),
			new Regex(@"^\s*_{2,4}$", RegexOptions.Compiled),
			new Regex(@"^-- $", RegexOptions.Compiled),
			new Regex(@"^\+{2,4}$", RegexOptions.Compiled),
			new Regex(@"^\={2,4}$", RegexOptions.Compiled),
			new Regex(@"^________________________________$", RegexOptions.Compiled), // 32-underscore separator (Outlook)

			// EN
			new Regex(@"^Sent from (?:\s*.+)$", RegexOptions.Compiled),
			new Regex(@"^Get Outlook for (?:\s*.+).*", RegexOptions.Multiline | RegexOptions.Compiled),
			new Regex(@"^Cheers,?!?$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled),
			new Regex(@"^Best wishes,?!?$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled),
			new Regex(@"^\w{0,20}\s?(\sand\s)?Regards,?!?！?$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled),

			// DE
			new Regex(@"^Von (?:\s*.+) gesendet$", RegexOptions.Compiled),
			new Regex(@"^Gesendet von (?:\s*.+) für (?:\s*.+)$", RegexOptions.Compiled),

			// DA
			new Regex(@"^Sendt fra (?:\s*.+)$", RegexOptions.Compiled),

			// FR
			new Regex(@"^Envoyé depuis (?:\s*.+)$", RegexOptions.Compiled),
			new Regex(@"^Envoyé de mon (?:\s*.+)$", RegexOptions.Compiled),
			new Regex(@"^Envoyé à partir de (?:\s*.+)$", RegexOptions.Compiled),
			new Regex(@"^Télécharger Outlook pour (?:\s*.+).*", RegexOptions.Multiline | RegexOptions.Compiled),
			new Regex(@"^Bien . vous,?!?$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled),
			new Regex(@"^\w{0,20}\s?cordialement,?!?$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled),
			new Regex(@"^Bonne (journ.e|soir.e)!?$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled),

			// ES
			new Regex(@"^Enviado desde (?:\s*.+)$", RegexOptions.Compiled),

			// IT
			new Regex(@"^-*\s*(In\sdata\s.+\s.+\n?scritto:{0,1})\s{0,1}-*$", RegexOptions.Multiline | RegexOptions.Compiled),

			// NL
			new Regex(@"^Verzonden vanaf (?:\s*.+)$", RegexOptions.Compiled),
			new Regex(@"^Verstuurd vanaf (?:\s*.+)$", RegexOptions.Compiled),
		};
	}
}
