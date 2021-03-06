﻿using MvvmFoundation.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TriCheer.Phoenix.SeqManager.Adaptor;
using TriCheer.Phoenix.SeqManager.SeqFile;

namespace Tricheer.Phoneix.SimpleSequenceEditor.VM
{
    class SequenceEditorVM : ObservableObject
    {
        #region ctor for singleton
        static SequenceEditorVM instance = new SequenceEditorVM();
        private SequenceEditorVM()
        {
            App.Messenger.Register<MethodInfoVM>(Messages.AddStepWithMethod, AddStepWithMethod);
        }
        public static SequenceEditorVM Instance { get { return instance; } }
        #endregion

        #region members
        SequenceFileVM seqFileVM;
        static SequenceFilePanelVM seqFilePanelVM = SequenceFilePanelVM.Instance;
        static TestModulePanelVM testModulePanelVM = TestModulePanelVM.Instance;
        static TestItemSettingsPanelVM stepSettingsPanelVM = TestItemSettingsPanelVM.Instance;
        static TestItemAdaptorPanelVM tiapVM = TestItemAdaptorPanelVM.Instance;
        #endregion

        #region props
        public SequenceFileVM SeqFileVM
        {
            get { return seqFileVM; }
            set { seqFileVM = value; RaisePropertyChanged("SeqFileVM"); }
        }
        public TestModulePanelVM TestModulePanelVM { get { return testModulePanelVM; } }
        public TestItemSettingsPanelVM TestItemSettingsPanelVM { get { return stepSettingsPanelVM; } }
        public TestItemAdaptorPanelVM TestItemAdaptorPanelVM { get { return tiapVM; } }
        public SequenceFilePanelVM SequenceFilePanelVM { get { return seqFilePanelVM; } }
        #endregion

        #region callbacks
        void AddStepWithMethod(MethodInfoVM miVM)
        {
            if (seqFileVM == null)
            {
                CreateSeqFileCmd.Execute(null);
            }
        }
        #endregion

        #region commands
        RelayCommand createSeqFileCmd = null;
        public ICommand CreateSeqFileCmd
        {
            get
            {
                if (createSeqFileCmd == null)
                {
                    createSeqFileCmd = new RelayCommand(OnCreateSeqFile);
                }
                return createSeqFileCmd;
            }
        }
        void OnCreateSeqFile()
        {
            //ISequenceFile seqFile = SequenceFileFactory.CreateSequenceFile();

            // for test
            SequenceFile seqFile = GenerateTestSequenceFile();

            SeqFileVM = new SequenceFileVM(seqFile);
        }

        // for Test
        SequenceFile GenerateTestSequenceFile()
        {
            SequenceFile seqFile = new SequenceFile();
            seqFile.Name = "TestSequenceFile";
            seqFile.Description = "This is a test sequenceFile.";
            seqFile.Comment = "Nothing";
            seqFile.Version.MarjorVersion = "1";

            ISequence mainSequence = SequenceFactory.CreateSequence(SequenceTypes.Normal);
            mainSequence.Name = "MainSequence1";
            mainSequence.Description = "Main sequence for test.";
            mainSequence.EnableLogging = false;
            mainSequence.TestTimeout = 3000;
            mainSequence.BreakPoint = true;

            IStep actionStep = StepFactory.CreateStep(StepTypes.Action);
            actionStep.Name = "Action step test";
            actionStep.Description = "this is a test action step";
            IAdaptor adaptor = AdaptorFactory.CreateAdaptor(AdaptorTypes.DotnetAdaptor);
            adaptor.MethodName = "Test";
            adaptor.TestModuleName = "DotNetTest.dll";
            adaptor.Parameters.Add(new DotNetParameter());
            adaptor.Parameters.Add(new DotNetParameter("parameter1"));
            adaptor.Parameters.Add(new DotNetParameter("parameter2"));
            actionStep.Adaptor = adaptor;

            IStep subActionStep = StepFactory.CreateStep(StepTypes.Action);
            subActionStep.Name = "SubAction step test";
            subActionStep.Description = "this is a sub test action step";

            actionStep.Children.Add(subActionStep);

            mainSequence.Children.Add(actionStep);
            mainSequence.Children.Add(subActionStep);
            mainSequence.Children.Add(subActionStep);

            ISequence mainSequence2 = SequenceFactory.CreateSequence(SequenceTypes.Normal);
            mainSequence2.Name = "MainSequence2";
            mainSequence2.Description = "Main sequence 2 for test.";
            mainSequence2.EnableLogging = false;
            mainSequence2.TestTimeout = 3000;
            mainSequence2.BreakPoint = true;

            seqFile.Sequences.Add(mainSequence);
            seqFile.Sequences.Add(mainSequence2);
            return seqFile;
        }

        RelayCommand addXttCmd = null;
        public ICommand AddXTTFileCmd
        {
            get
            {
                if (addXttCmd == null)
                {
                    addXttCmd = new RelayCommand(AddXTT);
                }
                return addXttCmd;
            }
        }

        void AddXTT()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Xtt Files (*.xtt)|*.xtt"
            };
            var result = openFileDialog.ShowDialog();
            string xttPath = string.Empty;
            if (result == true)
            {
                xttPath = openFileDialog.FileName;
            }
            else
            {
                return;
            }

            if (SeqFileVM == null)
            {
                CreateSeqFileCmd.Execute(null);
            }
            SeqFileVM.AddXttFile(xttPath);
        }
        #endregion
    }
}
