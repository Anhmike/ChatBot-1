using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;
using Irony.Ast;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace parser
{
    public partial class Form1 : Form
    {
        SpeechSynthesizer reader = new SpeechSynthesizer();
        SpeechRecognizer sr = new SpeechRecognizer();
        public class ExpressionGrammar : Irony.Parsing.Grammar
        {





            public ExpressionGrammar()
            {

                /// ----- Rule 1 ----- ///
                NonTerminal Quest = new NonTerminal("Quest");
                Quest.Rule = ToTerm("what") | "how" | "why";

                NonTerminal Verb = new NonTerminal("Verb");
                Verb.Rule = ToTerm("am") | "is" | "are";

                NonTerminal ProN = new NonTerminal("ProN");
                ProN.Rule = ToTerm("you") | "i" | "we" | "who" | "me";

                NonTerminal Action_Verb = new NonTerminal("Action_Verb");
                Action_Verb.Rule = ToTerm("eat") | "run";

                NonTerminal oneWord = new NonTerminal("oneWord");
                oneWord.Rule = ToTerm("fine") | "food";

                NonTerminal Adj = new NonTerminal("Adj");
                Adj.Rule = ToTerm("beautiful") | "pretty" | "intelligent";

                NonTerminal s_adj = new NonTerminal("s_adj");
                s_adj.Rule = ToTerm("prettier") | "smarter" | "elder";

                NonTerminal c_adj = new NonTerminal("c_adj");
                c_adj.Rule = ToTerm("than");

                NonTerminal adverb = new NonTerminal("adverb");
                adverb.Rule = ToTerm("fast") | "hard";

                NonTerminal supp_adverb = new NonTerminal("supp_adverb");
                supp_adverb.Rule = ToTerm("carefully") | "really";

                NonTerminal subj = new NonTerminal("subj");
                subj.Rule = ToTerm("robot") | "robo";

                NonTerminal noun = new NonTerminal("noun");
                noun.Rule = ToTerm("interests") | "hobbies" | "interest" | "hobby" | "problem";

                NonTerminal be_verb = new NonTerminal("be_verb");
                be_verb.Rule = ToTerm("your") | "mine" | "my";

                NonTerminal hob = new NonTerminal("hob");
                hob.Rule = ToTerm("football") | "cricket" | "basketball";

                NonTerminal irr_verb = new NonTerminal("irr_verb");
                irr_verb.Rule = ToTerm("ask") | "study" | "solve";

                NonTerminal ability = new NonTerminal("ability");
                ability.Rule = ToTerm("can");

                NonTerminal Gerunds = new NonTerminal("Gerunds");
                Gerunds.Rule = ToTerm("watching") | "seeing";

                NonTerminal imp = new NonTerminal("imp");
                imp.Rule = ToTerm("please");

                NonTerminal art = new NonTerminal("art");
                art.Rule = ToTerm("a") | "an";


                NonTerminal r1 = new NonTerminal("r1");
                r1.Rule =
                    Quest + Verb + ProN + "?" |
                    Quest + ProN + Action_Verb + "?" |
                    oneWord |
                    ProN + Verb + ProN + "?" |
                    ProN + Verb + Adj |
                    Verb + ProN + s_adj + c_adj + ProN + "?" |
                    ProN + Verb + s_adj + c_adj + ProN |
                    ProN + Verb + adverb |
                    ProN + Verb + supp_adverb + adverb |
                    ProN + Verb + adverb + subj |
                    ProN + Verb + supp_adverb + adverb + subj |
                    Quest + Verb + be_verb + noun + "?" |
                    be_verb + noun + Verb + hob |
                    ability + ProN + irr_verb + be_verb + noun + "?" |
                    ProN + Verb + Gerunds + "computer" + "screen" |
                    imp + "help" + ProN |
                    ProN + "have" + art + "umbrella";





                this.Root = r1;
                //this.Root = r2;
            }

        }

        public Form1()
        {
            Choices colors = new Choices();
            colors.Add(new string[] { "how are you", "fine", "blue" });

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(colors);

            // Create the Grammar instance.
            System.Speech.Recognition.Grammar g = new System.Speech.Recognition.Grammar(gb);
            sr.LoadGrammar(g);
            sr.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);

            InitializeComponent();
        }

        private void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
           // MessageBox.Show(e.Result.Text);
            analyseText(e.Result.Text);
        }






        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Irony.Parsing.Grammar grammar = new ExpressionGrammar();
                Parser parser = new Parser(grammar);
                ParseTree parseTree = parser.Parse(textBox1.Text);
                if (parseTree.Status.ToString() != "Error")
                {
                    listBox1.Items.Add("ME: " + parseTree.SourceText);
                    analyseText(parseTree.SourceText);
                }
                else
                {
                    listBox1.Items.Add("Bot: " + "Please use correct English Grammar Rule!");
                    reader.Speak("Please use correct English Grammar Rule!");
                }

                textBox1.Clear();
            }
        }

        private void analyseText(string p)
        {

            if (p.StartsWith("how") && p.Contains("you"))
            {
                listBox1.Items.Add("Bot: " + "I am fine, how are you?");
                reader.Speak("I am fine, how are you?");
            }

            else if (p.Contains("fine") || p.Contains("good"))
            {
                listBox1.Items.Add("Bot: " + "Cool .. :)");
                reader.Speak("Cool .. :)");
            }

            else if (p.StartsWith("what") && p.EndsWith("eat?"))
            {
                listBox1.Items.Add("Bot: " + "I eat code! What you eat?");
                reader.Speak("I eat code! What you eat?");
            }

            else if (p.StartsWith("who") && p.EndsWith("you?"))
            {
                listBox1.Items.Add("Bot: " + "I am Robot!");
                reader.Speak("I am Robot!");
            }
            else if (p.StartsWith("you") && (p.EndsWith("pretty") || p.EndsWith("beautiful") || p.EndsWith("intelligent")))
            {
                listBox1.Items.Add("Bot: " + "Hey! Thanks mate, you are adorable!");
                reader.Speak("Hey! Thanks mate, you are adorable!");
            }
            else if (p.StartsWith("are") && (p.Contains("smarter") || p.Contains("prettier") || p.Contains("elder")))
            {
                listBox1.Items.Add("Bot: " + "No dude, you are");
                reader.Speak("No dude, you are");
            }
            else if (p.StartsWith("you") && (p.Contains("smarter") || p.Contains("prettier") || p.Contains("elder")))
            {
                listBox1.Items.Add("Bot: " + "Thanks Dude!");
                reader.Speak("Thanks Dude!");
            }
            else if (p.StartsWith("you") && p.Contains("fast"))
            {
                listBox1.Items.Add("Bot: " + "O yes, thanks to your machine.");
                reader.Speak("O yes, thanks to your machine.");
            }
            else if (p.StartsWith("what") && (p.EndsWith("hobbies?") || p.EndsWith("interests?")))
            {
                listBox1.Items.Add("Bot: " + "I usually stay on circuits, interest? Good code!.");
                reader.Speak("I usually stay on circuits, interest? Good code!.");
            }
            else if (p.StartsWith("my") && (p.Contains("hobby") || p.Contains("interest")))
            {
                listBox1.Items.Add("Bot: " + "Oh great! So you are a sporty guy!");
                reader.Speak("Oh great! So you are a sporty guy!");
            }
            else if (p.StartsWith("can") && p.EndsWith("problem?"))
            {
                listBox1.Items.Add("Bot: " + "No, I do my own work.");
                reader.Speak("No, I do my own work.");
            }
            else if (p.StartsWith("i") && p.EndsWith("screen"))
            {
                listBox1.Items.Add("Bot: " + "I am watching you.");
                reader.Speak("I am watching you.");
            }
            else if (p.StartsWith("please") && p.EndsWith("me"))
            {
                listBox1.Items.Add("Bot: " + "I can't :(");
                reader.Speak("I can't :(");
            }
            else if (p.StartsWith("i") && p.EndsWith("umbrella"))
            {
                listBox1.Items.Add("Bot: " + "Thanks for the information.");
                reader.Speak("Thanks for the information.");
            }

        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();


            int itemIndex = e.Index;
            if (itemIndex >= 0 && itemIndex < listBox1.Items.Count)
            {
                Graphics g = e.Graphics;
                bool isItemSelected = listBox1.Items[e.Index].ToString().Contains("Bot:");
                // Background Color
                SolidBrush backgroundColorBrush = new SolidBrush((isItemSelected) ? Color.Red : Color.White);
                g.FillRectangle(backgroundColorBrush, e.Bounds);

                // Set text color
                string itemText = listBox1.Items[itemIndex].ToString();

                SolidBrush itemTextColorBrush = (isItemSelected) ? new SolidBrush(Color.White) : new SolidBrush(Color.Black);
                g.DrawString(itemText, e.Font, itemTextColorBrush, listBox1.GetItemRectangle(itemIndex).Location);

                // Clean up
                backgroundColorBrush.Dispose();
                itemTextColorBrush.Dispose();
            }

            e.DrawFocusRectangle();
        }

        private void listBox1_SizeChanged(object sender, EventArgs e)
        {
            //reader.Speak(listBox1.Items[listBox1.Items.Count].ToString());
        }

    }
}