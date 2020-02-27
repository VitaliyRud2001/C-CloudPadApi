using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Newtonsoft.Json;
using CloudPadApi.Http;
using CloudPadApi;
using CloudPad.Adapters;
using Android.Views;
using CloudPadApi.DbModels;

namespace CloudPad
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {

        ListView mListView;
        List<User_note> notes;

        ImageView plus_button;


        private CloudApi cloud;

        protected async override void OnCreate(Bundle savedInstanceState)
        {


           
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);


            plus_button = FindViewById<ImageView>(Resource.Id.plusButton);
            plus_button.Click += Plus_button_Click;

            cloud = JsonConvert.DeserializeObject<CloudApi>(Intent.GetStringExtra("CloudApi"));
            notes = await cloud.GetNotesAsync();
            if (notes != null)
            {
                
                mListView = FindViewById<ListView>(Resource.Id.myListView);

                NoteAdapter adapter = new NoteAdapter(this, notes);

                mListView.Adapter = adapter;
                mListView.ItemLongClick += MListView_ItemLongClick;
            }
            else
            {
                Toast.MakeText(this, cloud.Server_Msg,ToastLength.Long).Show();   
            }

         
        }

        private void Plus_button_Click(object sender, System.EventArgs e)
        {
            LayoutInflater layoutInfalter = LayoutInflater.From(this);
            View mView = layoutInfalter.Inflate(Resource.Layout.Edit_Dialog, null);
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
            alertDialogBuilder.SetView(mView);
            var userContent = mView.FindViewById<EditText>(Resource.Id.NoteEditText);

            alertDialogBuilder.SetCancelable(false).SetPositiveButton("OK"
                , async delegate {
                    var note = new Note_model {Note = userContent.Text };

                    var Created = await cloud.CreateNoteAsync(note);

                    if(Created)
                    {
                        notes.Add(new User_note { note = note.Note });
                        NoteAdapter noteAdapter = new NoteAdapter(this, notes);
                        mListView.Adapter = noteAdapter;
                    }
                    else
                    {
                        Toast.MakeText(this, cloud.Server_Msg, ToastLength.Long).Show();
                    }

                    
                });
            Android.Support.V7.App.AlertDialog alertDialog = alertDialogBuilder.Create();
            alertDialog.Show();
        }

        private async void MListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            View mView = layoutInflater.Inflate(Resource.Layout.Edit_Dialog, null);
            Android.Support.V7.App.AlertDialog.Builder alertDialogBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
            alertDialogBuilder.SetView(mView);

            var userContent = mView.FindViewById<EditText>(Resource.Id.NoteEditText);
            alertDialogBuilder.SetCancelable(false).SetPositiveButton("OK"
                ,async delegate {
                    var note = new Note_model { Note_Id = notes[e.Position].note_id, Note = userContent.Text };
                    bool Updated = await cloud.UpdateNoteAsync(note);
                   
                    if(Updated)
                    {
                        notes[e.Position].note = note.Note;
                        NoteAdapter noteAdapter = new NoteAdapter(this,notes);
                        mListView.Adapter = noteAdapter;
                
                    }
                    else
                    {
                        Toast.MakeText(this, cloud.Server_Msg, ToastLength.Long).Show();
                    }
                });


            Android.Support.V7.App.AlertDialog alertDialog = alertDialogBuilder.Create();
            alertDialog.Show();


        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}