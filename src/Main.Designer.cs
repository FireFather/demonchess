namespace DemonChess
{
    partial class Main
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.BoardPanel = new System.Windows.Forms.Panel();
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.mnGame = new System.Windows.Forms.ToolStripMenuItem();
            this.mnNewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.mnLoadGame = new System.Windows.Forms.ToolStripMenuItem();
            this.mnSaveGame = new System.Windows.Forms.ToolStripMenuItem();
            this.mnResignGame = new System.Windows.Forms.ToolStripMenuItem();
            this.mnExitGame = new System.Windows.Forms.ToolStripMenuItem();
            this.mnOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.ChooseWhiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChooseBlackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.mnTimeOption = new System.Windows.Forms.ToolStripMenuItem();
            this.mnUseOpeningBook = new System.Windows.Forms.ToolStripMenuItem();
            this.mnHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnIntroHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnHelpPlayer = new System.Windows.Forms.ToolStripMenuItem();
            this.UserPanel = new System.Windows.Forms.Panel();
            this.cmdThinker = new System.Windows.Forms.Button();
            this.btnLastMove = new System.Windows.Forms.Button();
            this.btnFirstMove = new System.Windows.Forms.Button();
            this.GridViewPgnFile = new System.Windows.Forms.DataGridView();
            this.Stt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WhiteMove = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BlackMove = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPreviousMove = new System.Windows.Forms.Button();
            this.btnNextMove = new System.Windows.Forms.Button();
            this.BlackPlayer = new System.Windows.Forms.TextBox();
            this.WhitePlayer = new System.Windows.Forms.TextBox();
            this.blackThinkingTimetxt = new System.Windows.Forms.TextBox();
            this.blackTotalTimetxt = new System.Windows.Forms.TextBox();
            this.whiteThinkingTimetxt = new System.Windows.Forms.TextBox();
            this.whiteTotalTimetxt = new System.Windows.Forms.TextBox();
            this.MatchTimer = new System.Windows.Forms.Timer(this.components);
            this.LVPvLine = new System.Windows.Forms.ListView();
            this.Menu.SuspendLayout();
            this.UserPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewPgnFile)).BeginInit();
            this.SuspendLayout();
            // 
            // BoardPanel
            // 
            this.BoardPanel.BackColor = System.Drawing.Color.DimGray;
            this.BoardPanel.Location = new System.Drawing.Point(22, 70);
            this.BoardPanel.Name = "BoardPanel";
            this.BoardPanel.Size = new System.Drawing.Size(402, 402);
            this.BoardPanel.TabIndex = 0;
            this.BoardPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.BoardPanel_Paint);
            // 
            // Menu
            // 
            this.Menu.AllowDrop = true;
            this.Menu.AutoSize = false;
            this.Menu.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Menu.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.Menu.GripMargin = new System.Windows.Forms.Padding(0);
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnGame,
            this.mnOptions,
            this.mnHelp});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.Menu.Size = new System.Drawing.Size(664, 28);
            this.Menu.Stretch = false;
            this.Menu.TabIndex = 1;
            this.Menu.Text = "File";
            // 
            // mnGame
            // 
            this.mnGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnNewGame,
            this.mnLoadGame,
            this.mnSaveGame,
            this.mnResignGame,
            this.mnExitGame});
            this.mnGame.ForeColor = System.Drawing.Color.Black;
            this.mnGame.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.mnGame.Name = "mnGame";
            this.mnGame.Padding = new System.Windows.Forms.Padding(8);
            this.mnGame.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.G)));
            this.mnGame.Size = new System.Drawing.Size(64, 28);
            this.mnGame.Text = "Game";
            this.mnGame.ToolTipText = "Game management";
            // 
            // mnNewGame
            // 
            this.mnNewGame.ForeColor = System.Drawing.Color.Black;
            this.mnNewGame.Name = "mnNewGame";
            this.mnNewGame.Size = new System.Drawing.Size(117, 22);
            this.mnNewGame.Text = "New";
            this.mnNewGame.Click += new System.EventHandler(this.mnNewGame_Click);
            // 
            // mnLoadGame
            // 
            this.mnLoadGame.ForeColor = System.Drawing.Color.Black;
            this.mnLoadGame.Name = "mnLoadGame";
            this.mnLoadGame.Size = new System.Drawing.Size(117, 22);
            this.mnLoadGame.Text = "Load";
            this.mnLoadGame.Click += new System.EventHandler(this.mnLoadGame_Click);
            // 
            // mnSaveGame
            // 
            this.mnSaveGame.ForeColor = System.Drawing.Color.Black;
            this.mnSaveGame.Name = "mnSaveGame";
            this.mnSaveGame.Size = new System.Drawing.Size(117, 22);
            this.mnSaveGame.Text = "Save";
            this.mnSaveGame.Click += new System.EventHandler(this.mnSaveGame_Click);
            // 
            // mnResignGame
            // 
            this.mnResignGame.ForeColor = System.Drawing.Color.Black;
            this.mnResignGame.Name = "mnResignGame";
            this.mnResignGame.Size = new System.Drawing.Size(117, 22);
            this.mnResignGame.Text = "Resign";
            this.mnResignGame.Click += new System.EventHandler(this.mnResign_Click);
            // 
            // mnExitGame
            // 
            this.mnExitGame.ForeColor = System.Drawing.Color.Black;
            this.mnExitGame.Name = "mnExitGame";
            this.mnExitGame.Size = new System.Drawing.Size(117, 22);
            this.mnExitGame.Text = "Exit";
            this.mnExitGame.Click += new System.EventHandler(this.mnExitGame_Click);
            // 
            // mnOptions
            // 
            this.mnOptions.AutoSize = false;
            this.mnOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChooseWhiteToolStripMenuItem,
            this.ChooseBlackToolStripMenuItem,
            this.mnLevel,
            this.mnTimeOption,
            this.mnUseOpeningBook});
            this.mnOptions.ForeColor = System.Drawing.Color.Black;
            this.mnOptions.Name = "mnOptions";
            this.mnOptions.Size = new System.Drawing.Size(60, 30);
            this.mnOptions.Text = "Options";
            // 
            // ChooseWhiteToolStripMenuItem
            // 
            this.ChooseWhiteToolStripMenuItem.Checked = true;
            this.ChooseWhiteToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChooseWhiteToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.ChooseWhiteToolStripMenuItem.Name = "ChooseWhiteToolStripMenuItem";
            this.ChooseWhiteToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.ChooseWhiteToolStripMenuItem.Text = "Play White";
            this.ChooseWhiteToolStripMenuItem.Click += new System.EventHandler(this.ChooseWhiteToolStripMenuItem_Click);
            // 
            // ChooseBlackToolStripMenuItem
            // 
            this.ChooseBlackToolStripMenuItem.Checked = true;
            this.ChooseBlackToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChooseBlackToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.ChooseBlackToolStripMenuItem.Name = "ChooseBlackToolStripMenuItem";
            this.ChooseBlackToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.ChooseBlackToolStripMenuItem.Text = "Play Black";
            this.ChooseBlackToolStripMenuItem.Click += new System.EventHandler(this.ChooseBlackToolStripMenuItem_Click);
            // 
            // mnLevel
            // 
            this.mnLevel.ForeColor = System.Drawing.Color.Black;
            this.mnLevel.Name = "mnLevel";
            this.mnLevel.Size = new System.Drawing.Size(163, 22);
            this.mnLevel.Text = "Level";
            this.mnLevel.Click += new System.EventHandler(this.mnLevel_Click);
            // 
            // mnTimeOption
            // 
            this.mnTimeOption.ForeColor = System.Drawing.Color.Black;
            this.mnTimeOption.Name = "mnTimeOption";
            this.mnTimeOption.Size = new System.Drawing.Size(163, 22);
            this.mnTimeOption.Text = "Time";
            this.mnTimeOption.Click += new System.EventHandler(this.mnTimeOption_Click);
            // 
            // mnUseOpeningBook
            // 
            this.mnUseOpeningBook.Checked = true;
            this.mnUseOpeningBook.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnUseOpeningBook.ForeColor = System.Drawing.Color.Black;
            this.mnUseOpeningBook.Name = "mnUseOpeningBook";
            this.mnUseOpeningBook.Size = new System.Drawing.Size(163, 22);
            this.mnUseOpeningBook.Text = "Opening book";
            this.mnUseOpeningBook.Click += new System.EventHandler(this.mnUseOpeningBook_Click);
            // 
            // mnHelp
            // 
            this.mnHelp.AutoSize = false;
            this.mnHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnIntroHelp,
            this.mnHelpPlayer});
            this.mnHelp.ForeColor = System.Drawing.Color.Black;
            this.mnHelp.Name = "mnHelp";
            this.mnHelp.Size = new System.Drawing.Size(60, 30);
            this.mnHelp.Text = "Help";
            // 
            // mnIntroHelp
            // 
            this.mnIntroHelp.ForeColor = System.Drawing.Color.Black;
            this.mnIntroHelp.Name = "mnIntroHelp";
            this.mnIntroHelp.Size = new System.Drawing.Size(112, 22);
            this.mnIntroHelp.Text = "About";
            this.mnIntroHelp.Click += new System.EventHandler(this.mnIntroHelp_Click);
            // 
            // mnHelpPlayer
            // 
            this.mnHelpPlayer.ForeColor = System.Drawing.Color.Black;
            this.mnHelpPlayer.Name = "mnHelpPlayer";
            this.mnHelpPlayer.Size = new System.Drawing.Size(112, 22);
            this.mnHelpPlayer.Text = "Guide";
            this.mnHelpPlayer.Click += new System.EventHandler(this.mnHelpPlayer_Click);
            // 
            // UserPanel
            // 
            this.UserPanel.BackColor = System.Drawing.Color.LightSteelBlue;
            this.UserPanel.Controls.Add(this.cmdThinker);
            this.UserPanel.Controls.Add(this.btnLastMove);
            this.UserPanel.Controls.Add(this.btnFirstMove);
            this.UserPanel.Controls.Add(this.GridViewPgnFile);
            this.UserPanel.Controls.Add(this.btnPreviousMove);
            this.UserPanel.Controls.Add(this.btnNextMove);
            this.UserPanel.Controls.Add(this.BlackPlayer);
            this.UserPanel.Controls.Add(this.WhitePlayer);
            this.UserPanel.Controls.Add(this.blackThinkingTimetxt);
            this.UserPanel.Controls.Add(this.blackTotalTimetxt);
            this.UserPanel.Controls.Add(this.whiteThinkingTimetxt);
            this.UserPanel.Controls.Add(this.whiteTotalTimetxt);
            this.UserPanel.Location = new System.Drawing.Point(22, 46);
            this.UserPanel.Margin = new System.Windows.Forms.Padding(0);
            this.UserPanel.Name = "UserPanel";
            this.UserPanel.Size = new System.Drawing.Size(622, 450);
            this.UserPanel.TabIndex = 2;
            this.UserPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.UserPanel_Paint);
            // 
            // cmdThinker
            // 
            this.cmdThinker.BackColor = System.Drawing.Color.LightSteelBlue;
            this.cmdThinker.FlatAppearance.BorderSize = 0;
            this.cmdThinker.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cmdThinker.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cmdThinker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdThinker.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdThinker.Image = global::DemonChess.Properties.Resources.engine;
            this.cmdThinker.Location = new System.Drawing.Point(503, 428);
            this.cmdThinker.Margin = new System.Windows.Forms.Padding(0);
            this.cmdThinker.Name = "cmdThinker";
            this.cmdThinker.Size = new System.Drawing.Size(34, 18);
            this.cmdThinker.TabIndex = 14;
            this.cmdThinker.UseVisualStyleBackColor = false;
            this.cmdThinker.Click += new System.EventHandler(this.CmdThinker_Click);
            // 
            // btnLastMove
            // 
            this.btnLastMove.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnLastMove.FlatAppearance.BorderSize = 0;
            this.btnLastMove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnLastMove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLastMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLastMove.Image = global::DemonChess.Properties.Resources.end;
            this.btnLastMove.Location = new System.Drawing.Point(563, 4);
            this.btnLastMove.Margin = new System.Windows.Forms.Padding(0);
            this.btnLastMove.Name = "btnLastMove";
            this.btnLastMove.Size = new System.Drawing.Size(25, 17);
            this.btnLastMove.TabIndex = 13;
            this.btnLastMove.UseVisualStyleBackColor = false;
            this.btnLastMove.Click += new System.EventHandler(this.btnLastMove_Click);
            // 
            // btnFirstMove
            // 
            this.btnFirstMove.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnFirstMove.FlatAppearance.BorderSize = 0;
            this.btnFirstMove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFirstMove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFirstMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFirstMove.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnFirstMove.Image = global::DemonChess.Properties.Resources.start;
            this.btnFirstMove.Location = new System.Drawing.Point(460, 4);
            this.btnFirstMove.Margin = new System.Windows.Forms.Padding(0);
            this.btnFirstMove.Name = "btnFirstMove";
            this.btnFirstMove.Size = new System.Drawing.Size(25, 17);
            this.btnFirstMove.TabIndex = 12;
            this.btnFirstMove.UseVisualStyleBackColor = false;
            this.btnFirstMove.Click += new System.EventHandler(this.btnFirstMove_Click);
            // 
            // GridViewPgnFile
            // 
            this.GridViewPgnFile.AllowUserToAddRows = false;
            this.GridViewPgnFile.AllowUserToDeleteRows = false;
            this.GridViewPgnFile.AllowUserToResizeColumns = false;
            this.GridViewPgnFile.AllowUserToResizeRows = false;
            this.GridViewPgnFile.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.GridViewPgnFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GridViewPgnFile.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridViewPgnFile.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.GridViewPgnFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridViewPgnFile.ColumnHeadersVisible = false;
            this.GridViewPgnFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Stt,
            this.WhiteMove,
            this.BlackMove});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.LightGoldenrodYellow;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GridViewPgnFile.DefaultCellStyle = dataGridViewCellStyle5;
            this.GridViewPgnFile.GridColor = System.Drawing.SystemColors.Control;
            this.GridViewPgnFile.Location = new System.Drawing.Point(424, 24);
            this.GridViewPgnFile.Name = "GridViewPgnFile";
            this.GridViewPgnFile.ReadOnly = true;
            this.GridViewPgnFile.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.GridViewPgnFile.RowHeadersVisible = false;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.LemonChiffon;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            this.GridViewPgnFile.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.GridViewPgnFile.RowTemplate.Height = 18;
            this.GridViewPgnFile.Size = new System.Drawing.Size(197, 402);
            this.GridViewPgnFile.TabIndex = 11;
            this.GridViewPgnFile.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewPgnFile_CellContentClick);
            // 
            // Stt
            // 
            this.Stt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Stt.HeaderText = "#";
            this.Stt.Name = "Stt";
            this.Stt.ReadOnly = true;
            // 
            // WhiteMove
            // 
            this.WhiteMove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WhiteMove.HeaderText = "White";
            this.WhiteMove.Name = "WhiteMove";
            this.WhiteMove.ReadOnly = true;
            // 
            // BlackMove
            // 
            this.BlackMove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BlackMove.HeaderText = "Black";
            this.BlackMove.Name = "BlackMove";
            this.BlackMove.ReadOnly = true;
            this.BlackMove.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // btnPreviousMove
            // 
            this.btnPreviousMove.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnPreviousMove.FlatAppearance.BorderSize = 0;
            this.btnPreviousMove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPreviousMove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPreviousMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviousMove.Image = global::DemonChess.Properties.Resources.back;
            this.btnPreviousMove.Location = new System.Drawing.Point(497, 4);
            this.btnPreviousMove.Margin = new System.Windows.Forms.Padding(0);
            this.btnPreviousMove.Name = "btnPreviousMove";
            this.btnPreviousMove.Size = new System.Drawing.Size(21, 17);
            this.btnPreviousMove.TabIndex = 9;
            this.btnPreviousMove.UseVisualStyleBackColor = false;
            this.btnPreviousMove.Click += new System.EventHandler(this.btnPreviousMove_Click);
            // 
            // btnNextMove
            // 
            this.btnNextMove.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnNextMove.FlatAppearance.BorderSize = 0;
            this.btnNextMove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextMove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextMove.Image = global::DemonChess.Properties.Resources.forward;
            this.btnNextMove.Location = new System.Drawing.Point(530, 4);
            this.btnNextMove.Margin = new System.Windows.Forms.Padding(0);
            this.btnNextMove.Name = "btnNextMove";
            this.btnNextMove.Size = new System.Drawing.Size(21, 17);
            this.btnNextMove.TabIndex = 8;
            this.btnNextMove.UseVisualStyleBackColor = false;
            this.btnNextMove.Click += new System.EventHandler(this.btnNextMove_Click);
            // 
            // BlackPlayer
            // 
            this.BlackPlayer.BackColor = System.Drawing.Color.Black;
            this.BlackPlayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BlackPlayer.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BlackPlayer.ForeColor = System.Drawing.SystemColors.Window;
            this.BlackPlayer.Location = new System.Drawing.Point(97, 2);
            this.BlackPlayer.Name = "BlackPlayer";
            this.BlackPlayer.ReadOnly = true;
            this.BlackPlayer.Size = new System.Drawing.Size(70, 21);
            this.BlackPlayer.TabIndex = 6;
            this.BlackPlayer.TabStop = false;
            this.BlackPlayer.Text = "Demon";
            this.BlackPlayer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BlackPlayer.WordWrap = false;
            // 
            // WhitePlayer
            // 
            this.WhitePlayer.BackColor = System.Drawing.Color.White;
            this.WhitePlayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WhitePlayer.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WhitePlayer.ForeColor = System.Drawing.Color.Black;
            this.WhitePlayer.Location = new System.Drawing.Point(97, 427);
            this.WhitePlayer.Name = "WhitePlayer";
            this.WhitePlayer.ReadOnly = true;
            this.WhitePlayer.Size = new System.Drawing.Size(70, 21);
            this.WhitePlayer.TabIndex = 5;
            this.WhitePlayer.TabStop = false;
            this.WhitePlayer.Text = "Human";
            this.WhitePlayer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.WhitePlayer.WordWrap = false;
            // 
            // blackThinkingTimetxt
            // 
            this.blackThinkingTimetxt.BackColor = System.Drawing.Color.Black;
            this.blackThinkingTimetxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.blackThinkingTimetxt.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blackThinkingTimetxt.ForeColor = System.Drawing.Color.White;
            this.blackThinkingTimetxt.Location = new System.Drawing.Point(235, 2);
            this.blackThinkingTimetxt.Name = "blackThinkingTimetxt";
            this.blackThinkingTimetxt.ReadOnly = true;
            this.blackThinkingTimetxt.Size = new System.Drawing.Size(70, 21);
            this.blackThinkingTimetxt.TabIndex = 4;
            this.blackThinkingTimetxt.TabStop = false;
            this.blackThinkingTimetxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.blackThinkingTimetxt.WordWrap = false;
            this.blackThinkingTimetxt.TextChanged += new System.EventHandler(this.blackThinkingTimetxt_TextChanged);
            // 
            // blackTotalTimetxt
            // 
            this.blackTotalTimetxt.BackColor = System.Drawing.Color.Black;
            this.blackTotalTimetxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.blackTotalTimetxt.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blackTotalTimetxt.ForeColor = System.Drawing.Color.White;
            this.blackTotalTimetxt.Location = new System.Drawing.Point(166, 2);
            this.blackTotalTimetxt.Multiline = true;
            this.blackTotalTimetxt.Name = "blackTotalTimetxt";
            this.blackTotalTimetxt.ReadOnly = true;
            this.blackTotalTimetxt.Size = new System.Drawing.Size(70, 21);
            this.blackTotalTimetxt.TabIndex = 3;
            this.blackTotalTimetxt.TabStop = false;
            this.blackTotalTimetxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.blackTotalTimetxt.WordWrap = false;
            // 
            // whiteThinkingTimetxt
            // 
            this.whiteThinkingTimetxt.BackColor = System.Drawing.Color.White;
            this.whiteThinkingTimetxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.whiteThinkingTimetxt.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.whiteThinkingTimetxt.ForeColor = System.Drawing.Color.Black;
            this.whiteThinkingTimetxt.Location = new System.Drawing.Point(235, 427);
            this.whiteThinkingTimetxt.Name = "whiteThinkingTimetxt";
            this.whiteThinkingTimetxt.ReadOnly = true;
            this.whiteThinkingTimetxt.Size = new System.Drawing.Size(70, 21);
            this.whiteThinkingTimetxt.TabIndex = 2;
            this.whiteThinkingTimetxt.TabStop = false;
            this.whiteThinkingTimetxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.whiteThinkingTimetxt.WordWrap = false;
            // 
            // whiteTotalTimetxt
            // 
            this.whiteTotalTimetxt.BackColor = System.Drawing.Color.White;
            this.whiteTotalTimetxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.whiteTotalTimetxt.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.whiteTotalTimetxt.ForeColor = System.Drawing.Color.Black;
            this.whiteTotalTimetxt.Location = new System.Drawing.Point(166, 427);
            this.whiteTotalTimetxt.Multiline = true;
            this.whiteTotalTimetxt.Name = "whiteTotalTimetxt";
            this.whiteTotalTimetxt.ReadOnly = true;
            this.whiteTotalTimetxt.Size = new System.Drawing.Size(70, 21);
            this.whiteTotalTimetxt.TabIndex = 1;
            this.whiteTotalTimetxt.TabStop = false;
            this.whiteTotalTimetxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.whiteTotalTimetxt.WordWrap = false;
            this.whiteTotalTimetxt.TextChanged += new System.EventHandler(this.WhiteTotalTimetxt_TextChanged);
            // 
            // MatchTimer
            // 
            this.MatchTimer.Enabled = true;
            this.MatchTimer.Tick += new System.EventHandler(this.MatchTimer_Tick);
            // 
            // LVPvLine
            // 
            this.LVPvLine.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.LVPvLine.BackColor = System.Drawing.Color.White;
            this.LVPvLine.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LVPvLine.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LVPvLine.GridLines = true;
            this.LVPvLine.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.LVPvLine.HideSelection = false;
            this.LVPvLine.Location = new System.Drawing.Point(21, 515);
            this.LVPvLine.MultiSelect = false;
            this.LVPvLine.Name = "LVPvLine";
            this.LVPvLine.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LVPvLine.Size = new System.Drawing.Size(622, 170);
            this.LVPvLine.TabIndex = 15;
            this.LVPvLine.UseCompatibleStateImageBehavior = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(664, 705);
            this.Controls.Add(this.BoardPanel);
            this.Controls.Add(this.LVPvLine);
            this.Controls.Add(this.UserPanel);
            this.Controls.Add(this.Menu);
            this.MainMenuStrip = this.Menu;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Demon Chess";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.UserPanel.ResumeLayout(false);
            this.UserPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewPgnFile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BoardPanel;
        private new System.Windows.Forms.MenuStrip Menu;
        private System.Windows.Forms.ToolStripMenuItem mnGame;
        private System.Windows.Forms.ToolStripMenuItem mnNewGame;
        private System.Windows.Forms.ToolStripMenuItem mnSaveGame;
        private System.Windows.Forms.ToolStripMenuItem mnExitGame;
        private System.Windows.Forms.ToolStripMenuItem mnOptions;
        private System.Windows.Forms.ToolStripMenuItem mnTimeOption;
        private System.Windows.Forms.ToolStripMenuItem mnHelp;
        private System.Windows.Forms.ToolStripMenuItem mnIntroHelp;
        private System.Windows.Forms.Panel UserPanel;
        private System.Windows.Forms.TextBox blackThinkingTimetxt;
        private System.Windows.Forms.TextBox blackTotalTimetxt;
        private System.Windows.Forms.TextBox whiteThinkingTimetxt;
        private System.Windows.Forms.TextBox whiteTotalTimetxt;
        private System.Windows.Forms.TextBox BlackPlayer;
        private System.Windows.Forms.TextBox WhitePlayer;
        private System.Windows.Forms.Timer MatchTimer;
        private System.Windows.Forms.ToolStripMenuItem mnLoadGame;
        private System.Windows.Forms.Button btnPreviousMove;
        private System.Windows.Forms.Button btnNextMove;
        private System.Windows.Forms.DataGridView GridViewPgnFile;
        private System.Windows.Forms.Button btnLastMove;
        private System.Windows.Forms.Button btnFirstMove;
        private System.Windows.Forms.ToolStripMenuItem ChooseWhiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ChooseBlackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnResignGame;
        private System.Windows.Forms.ToolStripMenuItem mnUseOpeningBook;
        private System.Windows.Forms.ToolStripMenuItem mnHelpPlayer;
        private System.Windows.Forms.ToolStripMenuItem mnLevel;
        private System.Windows.Forms.Button cmdThinker;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stt;
        private System.Windows.Forms.DataGridViewTextBoxColumn WhiteMove;
        private System.Windows.Forms.DataGridViewTextBoxColumn BlackMove;
        private System.Windows.Forms.ListView LVPvLine;
    }
}

