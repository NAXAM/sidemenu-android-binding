using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V7.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Yalantis.Com.Sidemenu.Interfaces;
using Yalantis.Com.Sidemenu.Util;
using static Android.Resource;
using Android.Support.V4.App;
using Java.Lang;

namespace SideMenuQs.Code
{
    class ContentFragment : Fragment, IScreenShotable
    {

        public const string CLOSE = "Close";
        public const string BUILDING = "Building";
        public const string BOOK = "Book";
        public const string PAINT = "Paint";
        public const string CASE = "Case";
        public const string SHOP = "Shop";
        public const string PARTY = "Party";
        public const string MOVIE = "Movie";

        private View containerView;
        protected ImageView mImageView;
        protected int res;
        private Bitmap bitmap;


        public static ContentFragment NewInstance(int resId)
        {
            ContentFragment contentFragment = new ContentFragment();
            Bundle bundle = new Bundle();
            bundle.PutInt(nameof(Android.Resource.Integer), resId);
            contentFragment.Arguments = bundle;
            return contentFragment;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            this.containerView = view.FindViewById(Resource.Id.container);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.fragment_main, container, false);
            mImageView = rootView.FindViewById<ImageView>(Resource.Id.image_content);
            mImageView.Clickable = true;
            mImageView.Focusable = true;
            mImageView.SetImageResource(res);

            return rootView;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            res = Arguments.GetInt(nameof(Android.Resource.Integer));
        }


        public Bitmap Bitmap
        {
            get { return bitmap; }
        }

        public void TakeScreenShot()
        {
            Thread thread = new Thread(() =>
            {
                Bitmap bitmap = Bitmap.CreateBitmap(containerView.Width, containerView.Height, Bitmap.Config.Argb8888);
                Canvas canvas = new Canvas(bitmap);
                containerView.Draw(canvas);
                this.bitmap = bitmap;
            });
        }
    }
}