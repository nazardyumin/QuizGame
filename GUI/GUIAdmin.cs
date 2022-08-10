using QuizGame.Helpers;
using Terminal.Gui;
using NStack;

namespace QuizGame.GUI
{
    public class GUIAdmin : GUIDefault
    {
        AdminFeatures _features;
        CreateQuiz createQuiz=new();
        private int iter = 0;
        private List<(string,int)> _memory;
        private string _memory_question;
        private string _memory_theme="";
        private int _memory_level=0;
        private string question_buffer = "";
        private List<(string, int)> buffers = new List<(string, int)>()
        {("",0),("",0),("",0),("",0),};
        public GUIAdmin(User user) : base(user)
        {
            _features = new(user);
            _changePass = _features.ChangePassword;
            _changeDate = _features.ChangeDateOfBirth;
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) MainMenuWindow()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            Application.Init();
            var top = Application.Top;
            var win = new Window("QuizGame")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            top.Add(win);
            var menu = new MenuBar(new MenuBarItem[] {new MenuBarItem ("_Menu", new MenuItem []
            {new MenuItem ("_Settings", "", () =>{action=SettingsWindow; top.Running = false; }),
             new MenuItem ("_Logout", "", () => { logout=true; top.Running = false; }),
             new MenuItem("_Quit", "", () => {  if (GUIHelper.Quit()) {keep_on = false;top.Running = false; } }) })});
            top.Add(menu);
            var hello = new Label(_role)
            {
                X = Pos.AnchorEnd(_role.Length) - 1,
                Y = Pos.AnchorEnd(1)
            };
            hello.ColorScheme = Colors.Menu;
            win.Add(hello);
            var create_quiz = new Button("Create New Quiz");
            create_quiz.X = Pos.Center();
            create_quiz.Y = 5;
            create_quiz.Clicked += () =>
            {
                action = CreateQuizWindow;
                top.Running = false;
            };
            var edit_quiz = new Button("Edit Quiz");
            edit_quiz.X = Pos.Center();
            edit_quiz.Y = Pos.Top(create_quiz) + 2;
            edit_quiz.Clicked += () =>
            {
                action = EditQuizWindow;
                top.Running = false;
            };
            win.Add(create_quiz, edit_quiz);
            Application.Run();
            return (keep_on, logout, back, action);
        }
        protected (bool keep_on, bool logout, bool back, SomeAction action) CreateQuizWindow()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            (bool keep_on, bool logout, bool back, SomeAction action) stop;
            do
            {
                stop = CreateQuizAddingItems();
                logout = stop.logout;
                keep_on = stop.keep_on;
                back=stop.back; 
            } while (logout == false && keep_on==true && back==false);
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) CreateQuizAddingItems()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            Application.Init();
            int count = createQuiz._quiz.Questions.Count();
            int our_position = 0;
            int zero_position = our_position - count;           
            var top = Application.Top;
            var win = new Window("QuizGame")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            top.Add(win);
            var menu = new MenuBar(new MenuBarItem[] {new MenuBarItem ("_Menu", new MenuItem []
            {new MenuItem ("_Logout", "", () => { logout=true; top.Running = false; }),
             new MenuItem("_Quit", "", () => {  if (GUIHelper.Quit()) {keep_on = false; top.Running = false;} }) })});
            top.Add(menu);
            var hello = new Label(_role)
            {
                X = Pos.AnchorEnd(_role.Length) - 1,
                Y = Pos.AnchorEnd(1)
            };
            hello.ColorScheme = Colors.Menu;
            win.Add(hello);
            var header = new Label("Creating New Quiz: ")
            {
                X = Pos.Center(),
                Y = 2
            };
            var theme = new Label("Theme: ")
            {
                X = 16,
                Y = Pos.Top(header) + 4
            };
            var theme_text = new TextField(_memory_theme)
            {
                X = Pos.Left(theme) + 8,
                Y = Pos.Top(theme),
                Width = 20
            };
            theme_text.Text= _memory_theme;
            var level = new Label("Level: ")
            {
                X = Pos.Left(theme),
                Y = Pos.Top(theme) + 3
            };
            var levels = new RadioGroup(new ustring[] { "Easy", "Normal", "Hard" }, 0)
            {
                X = Pos.Right(level) + 1,
                Y = Pos.Top(theme) + 2,
            };
            levels.SelectedItem = _memory_level;
            if (count > 0)
            {
                theme_text.CanFocus = false;
                levels.CanFocus = false;
            }
            var question = new Label($"Question {count + iter + 1}: ")
            {             
                Y = Pos.Top(level) +4
            };
            if (count+1>=10)
            {
                question.X = Pos.Left(level) - 6;
            }
            else
            {
                question.X = Pos.Left(level) - 5;
            }
            var question_text = new TextField(question_buffer)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(question),
                Width = 80
            };
            var answer1 = new Label("Answer 1: ")
            {
                X = Pos.Left(question) + 2,
                Y = Pos.Top(question) + 3
            };           
            var answer1_text = new TextField(buffers[0].Item1)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(answer1),
                Width = 40
            };
            var is_correct1 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
            {
                X = Pos.Right(answer1_text)+2,
                Y = Pos.Top(question) + 3
            };
            is_correct1.SelectedItem = buffers[0].Item2;
            var answer2 = new Label("Answer 2: ")
            {
                X = Pos.Left(answer1),
                Y = Pos.Top(answer1) + 3
            };      
            var answer2_text = new TextField(buffers[1].Item1)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(answer1)+3,
                Width = 40
            };
            var is_correct2 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
            {
                X = Pos.Left(is_correct1),
                Y = Pos.Top(is_correct1) + 3
            };
            is_correct2.SelectedItem = buffers[1].Item2;
            var answer3 = new Label("Answer 3: ")
            {
                X = Pos.Left(answer1),
                Y = Pos.Top(answer2) + 3
            };          
            var answer3_text = new TextField(buffers[2].Item1)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(answer2) + 3,
                Width = 40
            };
            var is_correct3 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
            {
                X = Pos.Left(is_correct1),
                Y = Pos.Top(is_correct2) + 3
            };
            is_correct3.SelectedItem = buffers[2].Item2;
            var answer4 = new Label("Answer 4: ")
            {
                X = Pos.Left(answer1),
                Y = Pos.Top(answer3) + 3
            };
            var answer4_text = new TextField(buffers[3].Item1)
            {
                X = Pos.Left(levels),
                Y = Pos.Top(answer3) + 3,
                Width = 40
            };
            var is_correct4 = new RadioGroup(new ustring[] { "Incorrect", "Correct" })
            {
                X = Pos.Left(is_correct1),
                Y = Pos.Top(is_correct3) + 3
            };
            is_correct3.SelectedItem = buffers[3].Item2;
            var add_item = new Button("Add Item")
            {
                X = Pos.Center(),
                Y = Pos.Top(is_correct4)+6
            };
            var step_back = new Button("Back")
            {
                X = Pos.Left(add_item) - 10,
                Y = Pos.Top(is_correct4) + 6
            };
            var step_forward = new Button("Forward")
             {
                 X = Pos.Right(add_item) + 2,
                 Y = Pos.Top(is_correct4) + 6
             };
            var edit_item = new Button("Edit Item")
            {
                X = Pos.Center(),
                Y = Pos.Top(is_correct4) + 6
            };
            var fast_forward = new Button(">>>")
            {
                X = Pos.Left(step_forward) + 13,
                Y = Pos.Top(is_correct4) + 6
            }; var fast_back = new Button("<<<")
            {
                X = Pos.Left(step_back) - 9,
                Y = Pos.Top(is_correct4) + 6
            };
            var questions_added = new Label($"Questions added: {count}")
            {                
                Y = Pos.Top(header) + 4
            };
            if (count<10)
            {
                questions_added.X = Pos.AnchorEnd(32);
            }
            else
            {
                questions_added.X = Pos.AnchorEnd(33);
            }
            var cancel = new Button("Cancel")
            {
                X = Pos.Center() - 11,
                Y = Pos.Top(add_item) + 4
            };
            cancel.Clicked += () =>
            {
                question_buffer = "";
                buffers[0] = ("", 0);
                buffers[1] = ("", 0);
                buffers[2] = ("", 0);
                buffers[3] = ("", 0);
                createQuiz._quiz.Questions.Clear();
                _memory_level = 0;
                _memory_theme = "";
                back = true;
                top.Running = false;
            };
            var save = new Button("Save")
            {
                X = Pos.Right(cancel) + 2,
                Y = Pos.Top(add_item) + 4
            };
            save.Clicked += () =>
            {
                createQuiz.SetTheme(_memory_theme);
                string what_level = "";
                switch (_memory_level)
                {
                    case 0:
                        what_level = "Easy";
                        break;
                    case 1:
                        what_level = "Normal";
                        break;
                    case 2:
                        what_level = "Hard";
                        break;
                    default:
                        break;
                };
                createQuiz.SetLevel(what_level);
                createQuiz.SaveToQuizListFile();
                back = true;
                _memory_theme = "";
                _memory_level = 0;
                top.Running = false;
            };
            if (iter+1<0)
            {
                fast_forward.Visible = true;              
            }
            else
            {
                fast_forward.Visible = false;
            }
            if (count>1 && iter-1 > zero_position)
            {
                fast_back.Visible = true;
            }
            else
            {
                fast_back.Visible = false;
            }
            if (count==0)
            {
                edit_item.Visible = false;
                add_item.Visible = true;
                step_back.Visible = false;
                step_forward.Visible = false;
            }
            else if (iter==zero_position && count>0)
            {
                add_item.Visible = false;
                edit_item.Visible = true;              
                step_back.Visible = false;
                step_forward.Visible = true;
            }
            else if (iter > zero_position && iter < 0)
            {
                add_item.Visible = false;
                edit_item.Visible = true;
                step_back.X = Pos.Right(edit_item) - 23;
                step_back.Visible = true;            
                step_forward.Visible = true;           
            }
            else 
            { 
                edit_item.Visible = false;
                add_item.Visible = true;
                step_back.Visible = true;
                step_forward.Visible = false;
            }
            fast_forward.Clicked += () =>
            {
                _memory_theme = theme_text.Text.ToString();
                _memory_level = levels.SelectedItem;
                iter = 0;
                RestoreInputData();      
                top.Running = false;
            };
            fast_back.Clicked += () =>
            {
                if (iter == 0) RememberInputData(question_text.Text.ToString(), answer1_text.Text.ToString(), answer2_text.Text.ToString(), answer3_text.Text.ToString(), answer4_text.Text.ToString(),
                                 is_correct1.SelectedItem, is_correct2.SelectedItem, is_correct3.SelectedItem, is_correct4.SelectedItem);
                iter = zero_position;
                int i = createQuiz._quiz.Questions.Count() + iter;
                question_buffer = createQuiz._quiz.Questions[i].Question;
                buffers[0] = (createQuiz._quiz.Questions[i].Answers[0].Answer, createQuiz._quiz.Questions[i].Answers[0].IsCorrect);
                buffers[1] = (createQuiz._quiz.Questions[i].Answers[1].Answer, createQuiz._quiz.Questions[i].Answers[1].IsCorrect);
                buffers[2] = (createQuiz._quiz.Questions[i].Answers[2].Answer, createQuiz._quiz.Questions[i].Answers[2].IsCorrect);
                buffers[3] = (createQuiz._quiz.Questions[i].Answers[3].Answer, createQuiz._quiz.Questions[i].Answers[3].IsCorrect);
                top.Running = false;
            };
            edit_item.Clicked += () => 
            {
                int i = createQuiz._quiz.Questions.Count() + iter;

                createQuiz._quiz.Questions[i].Question = question_text.Text.ToString();
                createQuiz._quiz.Questions[i].Answers[0].Answer = answer1_text.Text.ToString();
                createQuiz._quiz.Questions[i].Answers[1].Answer = answer2_text.Text.ToString();
                createQuiz._quiz.Questions[i].Answers[2].Answer = answer3_text.Text.ToString();
                createQuiz._quiz.Questions[i].Answers[3].Answer = answer4_text.Text.ToString();
                createQuiz._quiz.Questions[i].Answers[0].IsCorrect = is_correct1.SelectedItem;
                createQuiz._quiz.Questions[i].Answers[1].IsCorrect = is_correct2.SelectedItem;
                createQuiz._quiz.Questions[i].Answers[2].IsCorrect = is_correct3.SelectedItem;
                createQuiz._quiz.Questions[i].Answers[3].IsCorrect = is_correct4.SelectedItem;
                question_buffer = createQuiz._quiz.Questions[i].Question;
                buffers[0] = (createQuiz._quiz.Questions[i].Answers[0].Answer, createQuiz._quiz.Questions[i].Answers[0].IsCorrect);
                buffers[1] = (createQuiz._quiz.Questions[i].Answers[1].Answer, createQuiz._quiz.Questions[i].Answers[1].IsCorrect);
                buffers[2] = (createQuiz._quiz.Questions[i].Answers[2].Answer, createQuiz._quiz.Questions[i].Answers[2].IsCorrect);
                buffers[3] = (createQuiz._quiz.Questions[i].Answers[3].Answer, createQuiz._quiz.Questions[i].Answers[3].IsCorrect);
                top.Running = false;
            }; 
            add_item.Clicked += () =>
            {
                if (question_text.Text == "" || answer1_text.Text == "" || answer2_text.Text == "" || answer3_text.Text == "" || answer4_text.Text == "")
                {
                    MessageBox.ErrorQuery(30, 7, "Error!", "Not all fields are filled in!", "Ok");
                }
                else
                {
                    createQuiz.SetQuestion(question_text.Text.ToString());
                    createQuiz.SetAnswer(answer1_text.Text.ToString(), is_correct1.SelectedItem);
                    createQuiz.SetAnswer(answer2_text.Text.ToString(), is_correct2.SelectedItem);
                    createQuiz.SetAnswer(answer3_text.Text.ToString(), is_correct3.SelectedItem);
                    createQuiz.SetAnswer(answer4_text.Text.ToString(), is_correct4.SelectedItem);
                    createQuiz.AddItem();
                    question_buffer = "";
                    buffers[0] = ("", 0);
                    buffers[1] = ("", 0);
                    buffers[2] = ("", 0);
                    buffers[3] = ("", 0);
                    _memory_theme = theme_text.Text.ToString();
                    _memory_level = levels.SelectedItem;
                    top.Running = false;
                }
            };
            step_back.Clicked += () =>
            {
                _memory_theme = theme_text.Text.ToString();
                _memory_level = levels.SelectedItem;
                if (iter==0) RememberInputData(question_text.Text.ToString(), answer1_text.Text.ToString(), answer2_text.Text.ToString(), answer3_text.Text.ToString(), answer4_text.Text.ToString(),
                                  is_correct1.SelectedItem, is_correct2.SelectedItem, is_correct3.SelectedItem, is_correct4.SelectedItem);
                StepBack();
                top.Running = false;
            };
            step_forward.Clicked += () =>
            {
                _memory_theme = theme_text.Text.ToString();
                _memory_level = levels.SelectedItem;
                if (iter + 1 == 0) 
                {
                    iter = 0;
                    RestoreInputData();

                }
                else StepForward();
                top.Running = false;
            };   
            win.Add(header, theme, theme_text,level, levels,question,question_text,answer1, answer1_text, is_correct1,answer2,answer2_text, is_correct2,
                    answer3, answer3_text, is_correct3, answer4, answer4_text, is_correct4, add_item,step_back,edit_item, step_forward, fast_forward, questions_added, cancel,save, fast_back);
            Application.Run();
            return (keep_on, logout, back, action);
        }
        //окна для редактрования квизов
        private (bool keep_on, bool logout, bool back, SomeAction action) EditQuizWindow()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            return (keep_on, logout, back, action);
        }
        private (bool keep_on, bool logout, bool back, SomeAction action) EditQuizMenu()
        {
            bool logout = false;
            bool keep_on = true;
            bool back = false;
            SomeAction action = null;
            return (keep_on, logout, back, action);
        }
        public (bool keep_on, bool logout) AdminMenu()
        {
            bool keep_on = true;
            bool logout = false;
            do
            {
                var reply = MainMenuWindow();
                keep_on = reply.keep_on;
                logout = reply.logout;
                if (keep_on == false || logout == true) break;
                if (reply.action is not null)
                {
                    (bool keep_on, bool logout, bool back, SomeAction action) stop = reply.action.Invoke();
                    keep_on = stop.keep_on;
                    logout = stop.logout;
                }
            } while (logout == false && keep_on == true);
            return (keep_on, logout);
        }
        private void StepBack()
        {
            iter--;
            int i = createQuiz._quiz.Questions.Count() + iter;
            question_buffer= createQuiz._quiz.Questions[i].Question;
            buffers[0] = (createQuiz._quiz.Questions[i].Answers[0].Answer, createQuiz._quiz.Questions[i].Answers[0].IsCorrect);
            buffers[1] = (createQuiz._quiz.Questions[i].Answers[1].Answer, createQuiz._quiz.Questions[i].Answers[1].IsCorrect);
            buffers[2] = (createQuiz._quiz.Questions[i].Answers[2].Answer, createQuiz._quiz.Questions[i].Answers[2].IsCorrect);
            buffers[3] = (createQuiz._quiz.Questions[i].Answers[3].Answer, createQuiz._quiz.Questions[i].Answers[3].IsCorrect);
        }
        private void StepForward()
        {
            iter+=1;
            int i = createQuiz._quiz.Questions.Count() + iter;
            question_buffer = createQuiz._quiz.Questions[i].Question;
            buffers[0] = (createQuiz._quiz.Questions[i].Answers[0].Answer, createQuiz._quiz.Questions[i].Answers[0].IsCorrect);
            buffers[1] = (createQuiz._quiz.Questions[i].Answers[1].Answer, createQuiz._quiz.Questions[i].Answers[1].IsCorrect);
            buffers[2] = (createQuiz._quiz.Questions[i].Answers[2].Answer, createQuiz._quiz.Questions[i].Answers[2].IsCorrect);
            buffers[3] = (createQuiz._quiz.Questions[i].Answers[3].Answer, createQuiz._quiz.Questions[i].Answers[3].IsCorrect);
        }
        private void RememberInputData(string q, string a1, string a2, string a3, string a4, int c1, int c2, int c3, int c4)
        {
            _memory_question = q;
            _memory = new();
            _memory.Add((a1, c1));
            _memory.Add((a2, c2));
            _memory.Add((a3, c3));
            _memory.Add((a4, c4));
        }
        private void RestoreInputData()
        {
            question_buffer = _memory_question;
            buffers[0] = (_memory[0].Item1, _memory[0].Item2);
            buffers[1] = (_memory[1].Item1, _memory[1].Item2);
            buffers[2] = (_memory[2].Item1, _memory[2].Item2);
            buffers[3] = (_memory[3].Item1, _memory[3].Item2);
            _memory.Clear();
        }
    }
}

