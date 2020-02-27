using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CloudPadApi.Http;

namespace CloudPad.Adapters
{
    class NoteAdapter : BaseAdapter<User_note>
    {
        private List<User_note> list;

        private Context context;

        public NoteAdapter(Context context, List<User_note> list)
        {
            this.list = list;
            this.context = context;
        }

        public override int Count => list.Count;

        public override User_note this[int position] => list[position];


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if(view==null)
            {
                view = LayoutInflater.From(this.context).Inflate(Resource.Layout.NoteItem, null, false);

                TextView text = view.FindViewById<TextView>(Resource.Id.textView1);
                text.Text = list[position].note;

            }
            return view;

        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}