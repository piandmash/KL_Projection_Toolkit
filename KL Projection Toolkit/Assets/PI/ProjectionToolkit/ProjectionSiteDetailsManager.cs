﻿using UnityEngine;
using UnityEngine.EventSystems;
using PI.ProjectionToolkit.Models;
using TMPro;
using PI.ProjectionToolkit.UI;

namespace PI.ProjectionToolkit
{
    public class ProjectionSiteDetailsManager : UiDetailsBase
    {
        private ProjectionSite _projectionSite;
        public GameObject objTitle;
        public Sprite imgLocationIcon;
        public Sprite imgRefreshIcon;
        public UnityEngine.UI.Scrollbar scrollbar;
        public GameObject footerUpdateLocal;
        public GameObject footerInfoAndCreate;
        private ApplicationManager _applicationManager;
        public TextMeshProUGUI description;

        void Start()
        {
        }

        public void ShowFooterUpdateLocal(string descriptionText)
        {
            description.text = descriptionText;
            footerUpdateLocal.SetActive(true);
            footerInfoAndCreate.SetActive(false);
        }

        public void ShowFooterInfoAndCreate()
        {
            footerUpdateLocal.SetActive(false);
            footerInfoAndCreate.SetActive(true);
        }

        public void SetData(ProjectionSite projectionSite, ApplicationManager applicationManager)
        {
            _projectionSite = projectionSite;
            _applicationManager = applicationManager;
            this.colorAlert = applicationManager.colorAlert;
            Header header = objTitle.GetComponent<Header>();
            header.SetData(_projectionSite.name);
            scrollbar.value = 1; //set scrollbar to top

            //clear the transform
            foreach (Transform child in objList.transform) Destroy(child.gameObject);
            //build listing
            AddHeader("OVERVIEW");
            AddTextLine("NAME", _projectionSite.name);
            AddTextLine("VERSION", _projectionSite.version);
            switch (_projectionSite.status)
            {
                case ProjectionSiteStatus.Unknown:
                    TextLineButton btnUnknown = AddTextLineButton("VERSION STATUS", "UNKNOWN", imgRefreshIcon);
                    btnUnknown.OnButtonClick += BtnUnknown_OnButtonClick;
                    break;
                case ProjectionSiteStatus.OutOfDate:
                    AddTextLine("VERSION STATUS", "OUT OF DATE", false, true);
                    break;
                case ProjectionSiteStatus.NotOnServer:
                    AddTextLine("VERSION STATUS", "NOT ON SERVER", false, true);
                    break;
                case ProjectionSiteStatus.UpToDate:
                    AddTextLine("VERSION STATUS", "UP TO DATE");
                    break;
                case ProjectionSiteStatus.NewOnServer:
                    AddTextLine("VERSION STATUS", "NEW");
                    break;
            }
            AddTextLine("LOCATION", _projectionSite.location.name);
            AddTextLine("CITY/TOWN", _projectionSite.location.town);
            TextLineButton btnLoc = AddTextLineButton("GEO LOCATION", _projectionSite.location.geoLocation.latLng, imgLocationIcon);
            btnLoc.OnButtonClick += BtnLoc_OnButtonClick;
            AddTextLine("PROJECTOR STACKS", _projectionSite.projectors.Count.ToString());
            AddTextLine("CAMERAS", _projectionSite.cameras.Count.ToString());
            AddTextLine("NOTES", _projectionSite.notes, true);
            AddSeperator();

            AddProjectorStack(_projectionSite.projectors);

            AddCamera(_projectionSite.cameras, true);

            AddHeader("DETAILS");
            AddTextLine("ID", _projectionSite.id);
            AddTextLine("VERSION ID", _projectionSite.versionId);
            AddTextLine("ASSET BUNDLE", _projectionSite.assetBundleName);
            var textLineProjectResources = AddTextLine("PROJECT RECOURSES", string.Join("\n", _projectionSite.projectResources.ToArray()), true);
            textLineProjectResources.AlignValue();
            var textLineSiteResources = AddTextLine("SITE RECOURSES", string.Join("\n", _projectionSite.siteResources.ToArray()), true);
            textLineSiteResources.AlignValue();
            AddSeperator();


            AddHeader("AUDIT");
            AddTextLine("LAST UPDATED", _projectionSite.updatedAsString);
            AddTextLine("UPDATED BY", _projectionSite.updatedBy);
            AddTextLine("CREATED", _projectionSite.createdAsString);
            AddTextLine("CREATED BY", _projectionSite.createdBy);
            AddSeperator();
        }

        private void BtnLoc_OnButtonClick()
        {
            string url = string.IsNullOrEmpty(_projectionSite.location.mapUrl) ? "https://www.google.co.uk/maps/@" + _projectionSite.location.geoLocation.latLng + ",14z" : _projectionSite.location.mapUrl;
            Application.OpenURL(url);
        }

        private void BtnUnknown_OnButtonClick()
        {
            _applicationManager.ProjectInfoModalRefreshClick();
        }
    }
}