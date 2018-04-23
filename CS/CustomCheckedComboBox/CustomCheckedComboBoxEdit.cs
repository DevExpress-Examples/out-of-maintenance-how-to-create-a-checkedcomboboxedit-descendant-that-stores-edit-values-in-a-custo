using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors.Registrator;
using System.ComponentModel;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;

namespace CustomCheckedComboBox
{
    [UserRepositoryItem("RegisterCustomCheckedComboBoxEdit")]
    public class RepositoryItemCustomCheckedComboBoxEdit : RepositoryItemCheckedComboBoxEdit
    {
        static RepositoryItemCustomCheckedComboBoxEdit() { RegisterCustomCheckedComboBoxEdit(); }

        public RepositoryItemCustomCheckedComboBoxEdit() { }

        public const string CustomCheckedComboBoxEditName = "CustomCheckedComboBoxEdit";

        public override string EditorTypeName { get { return CustomCheckedComboBoxEditName; } }

        public static void RegisterCustomCheckedComboBoxEdit()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomCheckedComboBoxEditName,
              typeof(CustomCheckedComboBoxEdit), typeof(RepositoryItemCustomCheckedComboBoxEdit),
              typeof(PopupContainerEditViewInfo), new ButtonEditPainter(), true));
        }

        public override void Assign(RepositoryItem item)
        {
            base.Assign(item);
            RepositoryItemCustomCheckedComboBoxEdit source = item as RepositoryItemCustomCheckedComboBoxEdit;
            Events.AddHandler(_convertCheckStateToEditValue, source.Events[_convertCheckStateToEditValue]);
            Events.AddHandler(_convertEditValueToCheckState, source.Events[_convertEditValueToCheckState]);
        }

        static readonly object _convertCheckStateToEditValue = new object();

        protected override void PreQueryResultValue(QueryResultValueEventArgs e)
        {
            if (CanRaiseConvertCheckStateToEditValue)
            {
                ConvertCheckStateToEditValueEventArgs ea = new ConvertCheckStateToEditValueEventArgs(Items.Count);
                for (int i = 0; i < Items.Count; i++)
                    if (Items[i].CheckState == System.Windows.Forms.CheckState.Checked)
                        ea.CheckedState[i] = Items[i].Enabled;
                RaiseConvertCheckStateToEditValue(ea);
                e.Value = ea.EditValue;
            }
        }

        protected internal virtual string makeNormalValue(bool[] chekers)
        {
            string res = "";
            if (chekers != null)
            {
                for (int i = 0; i < chekers.Length; i++)
                    if (chekers[i]) res = res + Items[i].Value as string + SeparatorChar + " ";
            }
            if (res.Length > 2) res = res.Substring(0, res.Length - 2);
            return res;
        }

        protected override void PreQueryDisplayText(QueryDisplayTextEventArgs e)
        {
            if (CanRaiseConvertEditValueToCheckState && e.EditValue != null)
            {
                ConvertEditValueToCheckStateEventArgs ea = new ConvertEditValueToCheckStateEventArgs(e.EditValue as string, Items.Count);                
                RaiseConvertEditValueToCheckState(ea);
                e.DisplayText = makeNormalValue(ea.CheckedState);
            }

            base.PreQueryDisplayText(e);

        }

        public event ConvertCheckStateToEditValueEventHandler ConvertCheckStateToEditValue
        {
            add { this.Events.AddHandler(_convertCheckStateToEditValue, value); }
            remove { this.Events.RemoveHandler(_convertCheckStateToEditValue, value); }
        }

        protected internal virtual void RaiseConvertCheckStateToEditValue(ConvertCheckStateToEditValueEventArgs e)
        {
            ConvertCheckStateToEditValueEventHandler handler = (ConvertCheckStateToEditValueEventHandler)Events[_convertCheckStateToEditValue];
            if (handler != null) handler(GetEventSender(), e);
        }
        internal bool CanRaiseConvertCheckStateToEditValue { get { return (ConvertCheckStateToEditValueEventHandler)Events[_convertCheckStateToEditValue] != null; } }


        static readonly object _convertEditValueToCheckState = new object();

        public event ConvertEditValueToCheckStateEventHandler ConvertEditValueToCheckState
        {
            add { this.Events.AddHandler(_convertEditValueToCheckState, value); }
            remove { this.Events.RemoveHandler(_convertEditValueToCheckState, value); }
        }

        protected internal virtual void RaiseConvertEditValueToCheckState(ConvertEditValueToCheckStateEventArgs e)
        {
            ConvertEditValueToCheckStateEventHandler handler = (ConvertEditValueToCheckStateEventHandler)Events[_convertEditValueToCheckState];
            if (handler != null) handler(GetEventSender(), e);
        }
        internal bool CanRaiseConvertEditValueToCheckState { get { return (ConvertEditValueToCheckStateEventHandler)Events[_convertEditValueToCheckState] != null; } }

    }

    public delegate void ConvertCheckStateToEditValueEventHandler(Object sender, ConvertCheckStateToEditValueEventArgs e);

    public class ConvertCheckStateToEditValueEventArgs : EventArgs
    {
        string editValue;
        bool[] checkedState;
        public ConvertCheckStateToEditValueEventArgs(int count)
        {
            this.checkedState = new bool[count];
        }
        public string EditValue
        {
            get { return editValue; }
            set { editValue = value; }
        }
        public bool[] CheckedState
        {
            get { return checkedState; }
            set { checkedState = value; }
        }
    }

    public delegate void ConvertEditValueToCheckStateEventHandler(Object sender, ConvertEditValueToCheckStateEventArgs e);

    public class ConvertEditValueToCheckStateEventArgs : EventArgs
    {
        string editValue;
        bool[] checkedState;
        public ConvertEditValueToCheckStateEventArgs(string value, int count) 
        { 
            this.editValue = value;
            this.checkedState = new bool[count];
        }
        public string EditValue
        {
            get { return editValue; }
            set { editValue = value; }
        }
        public bool[] CheckedState
        {
            get { return checkedState; }
            set { checkedState = value; }
        }
    }

    public class CustomCheckedComboBoxEdit : CheckedComboBoxEdit
    {
        static CustomCheckedComboBoxEdit() { RepositoryItemCustomCheckedComboBoxEdit.RegisterCustomCheckedComboBoxEdit(); }

        public CustomCheckedComboBoxEdit() : base() { isShowPopup = false; }

        public override string EditorTypeName { get { return RepositoryItemCustomCheckedComboBoxEdit.CustomCheckedComboBoxEditName; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemCustomCheckedComboBoxEdit Properties { get { return base.Properties as RepositoryItemCustomCheckedComboBoxEdit; } }

        bool isShowPopup;

        public override object EditValue
        {
            get
            {
                if (!isShowPopup) return base.EditValue;
                if (Properties.CanRaiseConvertEditValueToCheckState && base.EditValue != null)
                {
                    ConvertEditValueToCheckStateEventArgs e = new ConvertEditValueToCheckStateEventArgs(base.EditValue as string, Properties.Items.Count );
                    Properties.RaiseConvertEditValueToCheckState(e);
                    return Properties.makeNormalValue(e.CheckedState);
                }
                else
                    return base.EditValue;
            }
            set
            {
                base.EditValue = value;
            }
        }


        protected override void DoShowPopup()
        {
            isShowPopup = true;
            base.DoShowPopup();
            isShowPopup = false;
            PopupForm.OldEditValue = EditValue;
        }


    }
}
