using System.IO;
using System.Linq;
using Xunit;

#pragma warning disable xUnit2000
#pragma warning disable xUnit2004
#pragma warning disable xUnit2012
#pragma warning disable xUnit2013

namespace EmailReplyParser.Tests
{
	public sealed class EmailParserTests
	{
		private static readonly string COMMON_FIRST_FRAGMENT =
			@"Fusce bibendum, quam hendrerit sagittis tempor, dui turpis tempus erat, pharetra sodales ante sem sit amet metus.
Nulla malesuada, orci non vulputate lobortis, massa felis pharetra ex, convallis consectetur ex libero eget ante.
Nam vel turpis posuere, rhoncus ligula in, venenatis orci. Duis interdum venenatis ex a rutrum.
Duis ut libero eu lectus consequat consequat ut vel lorem. Vestibulum convallis lectus urna,
et mollis ligula rutrum quis. Fusce sed odio id arcu varius aliquet nec nec nibh.".Replace("\r\n", "\n");

		private static Email Get_email(string name)
		{
			var data = File.ReadAllText(Path.Combine("resources", $"{name}.txt"));

			return EmailReplyParser.Read(data);
		}

		private static string Get_raw_email(string name)
		{
			var data = File.ReadAllText(Path.Combine("resources", $"{name}.txt"));
			return data;
		}

		[Fact]
		public void Test_complex_body_with_only_one_fragment()
		{
			var email = Get_email("email_5");

			var fragments = email.Fragments;

			Assert.Equal(1, fragments.Length);
		}

		[Fact]
		public void Test_deals_with_multiline_reply_headers()
		{
			var email = Get_email("email_6");

			var fragments = email.Fragments;

			Assert.Equal(true, @"^I get".Test(fragments[0]));
			Assert.Equal(true, @"^On".Test(fragments[1]));
			Assert.Equal(true, @"Was this".Test(fragments[1]));
		}

		[Fact]
		public void Test_email_22()
		{
			var email = Get_email("email_22");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_23()
		{
			var email = Get_email("email_23");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_da_into_italian()
		{
			var email = Get_email("email_13");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_emoji()
		{
			var email = Get_email("email_emoji");

			Assert.Equal(email.GetVisibleText(), "🎉\n\n—\nJohn Doe\nCEO at Pandaland\n\n@pandaland");
		}

		[Fact]
		public void Test_email_finnish()
		{
			var email = Get_email("email_finnish");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_german()
		{
			var email = Get_email("email_german");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_gmail_no()
		{
			var email = Get_email("email_norwegian_gmail");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_header_polish()
		{
			var email = Get_email("email_14");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_header_polish_with_date_in_iso8601()
		{
			var email = Get_email("email_17");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_header_polish_with_dnia_and_napisala()
		{
			var email = Get_email("email_16");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_not_a_signature()
		{
			var email = Get_email("email_not_a_signature");

			Assert.False(email.Fragments.Any(x => x.IsSignature));
		}

		[Fact]
		public void Test_email_outlook_en()
		{
			var email = Get_email("email_18");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_portuguese()
		{
			var email = Get_email("email_portuguese");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_sent_from_my()
		{
			var email = Get_email("email_15");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_square_brackets()
		{
			var email = Get_email("email_12");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_whitespace_before_header()
		{
			var email = Get_email("email_11");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_with_correct_signature()
		{
			var email = Get_email("correct_sig");

			var fragments = email.Fragments;

			Assert.Equal(2, fragments.Length);
			Assert.Equal(false, fragments[1].IsQuoted);
			Assert.Equal(false, fragments[0].IsSignature);
			Assert.Equal(true, fragments[1].IsSignature);
			Assert.Equal(false, fragments[0].IsHidden);
			Assert.Equal(true, fragments[1].IsHidden);

			Assert.Equal(true, @"^--\nrick".Test(fragments[1]));
		}

		[Fact]
		public void Test_email_with_dutch()
		{
			var email = Get_email("email_8");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_with_hotmail()
		{
			var email = Get_email("email_10");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_with_italian()
		{
			var email = Get_email("email_7");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_email_with_signature()
		{
			var email = Get_email("email_9");

			var fragments = email.Fragments;

			Assert.Equal(COMMON_FIRST_FRAGMENT, fragments[0].ToString().Trim());
		}

		[Fact]
		public void Test_one_is_not_one()
		{
			var email = Get_email("email_one_is_not_on");

			var fragments = email.Fragments;

			Assert.Equal(true, @"One outstanding question".Test(fragments[0]));
			Assert.Equal(true, @"^On Oct 1, 2012".Test(fragments[1]));
		}

		[Fact]
		public void Test_reads_bottom_post()
		{
			var email = Get_email("email_2");

			var fragments = email.Fragments.ToArray();
			Assert.Equal(6, fragments.Count());

			Assert.Equal("Hi,", fragments[0].Content);
			Assert.Equal(true, @"^On [^\:]+\:".Test(fragments[1]));
			Assert.Equal(true, @"^You can list".Test(fragments[2]));
			Assert.Equal(true, @"^>".Test(fragments[3]));
			Assert.Equal(true, @"^_".Test(fragments[5]));
		}

		[Fact]
		public void Test_reads_email_with_signature_with_no_empty_line_above()
		{
			var email = Get_email("sig_no_empty_line");

			var fragments = email.Fragments;

			Assert.Equal(2, fragments.Length);
			Assert.Equal(false, fragments[0].IsQuoted);
			Assert.Equal(false, fragments[1].IsQuoted);

			Assert.Equal(false, fragments[0].IsSignature);
			Assert.Equal(true, fragments[1].IsSignature);

			Assert.Equal(false, fragments[0].IsHidden);
			Assert.Equal(true, fragments[1].IsHidden);

			Assert.Equal(true, @"^--\nrick".Test(fragments[1]));
		}

		[Fact]
		public void Test_reads_simple_body()
		{
			var reply = Get_email("email_1");

			Assert.Equal(2, reply.Fragments.Length);

			Assert.True(reply.Fragments.All(x => !x.IsQuoted));

			Assert.Equal(new[] {false, true}, reply.Fragments.Select(x => x.IsHidden));

			Assert.Equal(
				"Hi folks\n\nWhat is the best way to clear a Riak bucket of all key, values after\nrunning a test?\nI am currently using the Java HTTP API.\n\n-Abhishek Kona\n\n",
				reply.Fragments[0].Content);
		}

		[Fact]
		public void Test_reads_top_post()
		{
			var email = Get_email("email_3");

			var fragments = email.Fragments.ToArray();
			Assert.Equal(4, fragments.Count());
			Assert.Equal(false, fragments[0].IsQuoted);
			Assert.Equal(true, fragments[1].IsQuoted);
			Assert.Equal(false, fragments[2].IsQuoted);
			Assert.Equal(false, fragments[3].IsQuoted);
			Assert.Equal(false, fragments[0].IsSignature);
			Assert.Equal(false, fragments[1].IsSignature);
			Assert.Equal(false, fragments[2].IsSignature);
			Assert.Equal(true, fragments[3].IsSignature);
			Assert.Equal(false, fragments[0].IsHidden);
			Assert.Equal(true, fragments[1].IsHidden);
			Assert.Equal(true, fragments[2].IsHidden);
			Assert.Equal(true, fragments[3].IsHidden);
			Assert.Equal(true, @"^Oh thanks.\n\nHaving".Test(fragments[0]));
			Assert.Equal(true, @"^On [^\:]+\:".Test(fragments[1]));
			Assert.Equal(true, @"^_".Test(fragments[3]));
		}

		[Fact]
		public void Test_recognizes_data_string_above_quote()
		{
			var email = Get_email("email_4");

			var fragments = email.Fragments;

			Assert.Equal(true, @"^Awesome".Test(fragments[0]));
			Assert.Equal(true, @"^On".Test(fragments[1]));
			Assert.Equal(true, @"Loader".Test(fragments[1]));
		}

		[Theory]
		[InlineData("Les Misאֳrables")]
		public void Test_graphemes_preserved_in_reversing(string text)
		{
			var email = EmailReplyParser.Read(text);

			Assert.Equal(text, email.Fragments[0].Content);
		}

		[Fact]
		public void Test_sent_from()
		{
			var email = Get_email("email_sent_from");

			Assert.Equal(email.GetVisibleText(),
				"Hi it can happen to any texts you type, as long as you type in between words or paragraphs.\n");
		}
	}
}