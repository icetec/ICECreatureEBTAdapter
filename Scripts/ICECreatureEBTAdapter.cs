// ##############################################################################
//
// ICECreatureEBTAdapter.cs
// Version 1.1
//
// © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.ice-technologies.com
// mailto:support@ice-technologies.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.Utilities;

using ICE.Creatures;
using ICE.Creatures.Objects;
using ICE.Creatures.Utilities;

using EnergyBarToolkit;


namespace ICE.Creatures.Adapter
{
	public enum ICEEnergyBarDataType{
		Age,

		Fitness,
		Health,
		Armor,
		Stamina,
		Power,

		Damage,
		Hunger,
		Thirst,
		Stress,
		Debility,

		Aggressivity,
		Anxiety,
		Experience,
		Nosiness,

		SenseVisual,
		SenseOlfactory,
		SenseGustatory,
		SenseAuditory,
		SenseTactile
	}

	[System.Serializable]
	public class ICEEnergyBarObject : System.Object 
	{
		public ICEEnergyBarObject(){}
		public ICEEnergyBarObject( ICEEnergyBarObject _bar )
		{
			if( _bar == null )
				return;
			
			ReferenceObject = _bar.ReferenceObject;
			Value = _bar.Value;
			Offset = _bar.Offset;
			VerticalOffset = _bar.VerticalOffset;
		}

		public EnergyBar ReferenceObject = null;
		public ICEEnergyBarDataType Value = ICEEnergyBarDataType.Fitness;
		public Vector3 Offset = Vector3.zero;
		public float VerticalOffset = 0;

		private ICECreatureEBTAdapter m_Adapter = null;
		private GameObject m_EnergyBarGameObject = null;
		private EnergyBar m_EnergyBar = null;

		public void Init( ICECreatureEBTAdapter _adapter )
		{
			m_Adapter = _adapter;

			if( m_Adapter == null  || m_Adapter.Controller == null || m_Adapter.Display == null || ReferenceObject == null )
				return;

			if( m_EnergyBarGameObject == null )
				m_EnergyBarGameObject = GameObject.Instantiate( ReferenceObject.gameObject );

			if( m_EnergyBarGameObject != null )
			{
				m_EnergyBarGameObject.transform.SetParent( m_Adapter.Display.transform, false );
				EnergyBarFollowObject _EnergyBarFollowObject = m_EnergyBarGameObject.GetComponent<EnergyBarFollowObject>();

				if( _EnergyBarFollowObject )
				{
					_EnergyBarFollowObject.followObject = m_Adapter.transform.gameObject;
					_EnergyBarFollowObject.worldCamera = new ObjectFinder(typeof(Camera), "/Main Camera", "MainCamera", ObjectFinder.Method.ByTag);
					_EnergyBarFollowObject.offset = Offset;
					_EnergyBarFollowObject.offset.y += VerticalOffset;
				}

			
				m_EnergyBar = m_EnergyBarGameObject.GetComponent<EnergyBar>();
				if( m_EnergyBar ) 
				{
					m_EnergyBar.valueMin = 0;
					m_EnergyBar.valueMax = (int)GetMaxValue( Value );
					m_EnergyBar.animationEnabled = false;
				}
			}

		}

		public void UpdateEnergyBar()
		{
			if( m_Adapter == null  || m_Adapter.Controller == null || m_EnergyBar == null )
				return;
			
			m_EnergyBar.valueCurrent = (int)GetValue( Value );

			#if UNITY_EDITOR
				EnergyBarFollowObject _EnergyBarFollowObject = m_EnergyBarGameObject.GetComponent<EnergyBarFollowObject>();
				if( _EnergyBarFollowObject != null )
				{
					_EnergyBarFollowObject.offset = Offset;
					_EnergyBarFollowObject.offset.y += VerticalOffset;

				Quaternion _rot = _EnergyBarFollowObject.transform.rotation;

				_rot.z = 90;
				}
			#endif
		}

		public void EnableEnergyBar( ICECreatureEBTAdapter _adapter ){
			Init( _adapter );

			if( m_EnergyBarGameObject != null )
				m_EnergyBarGameObject.SetActive(true);
		}

		public void DisableEnergyBar(){
			if ( m_EnergyBarGameObject != null) 
				m_EnergyBarGameObject.SetActive(false);
		}

		public void DestroyEnergyBar(){
			if( m_EnergyBarGameObject != null ) {
				GameObject.Destroy( m_EnergyBarGameObject );
			}
		}

		private float GetValue( ICEEnergyBarDataType _type )
		{
			switch( _type ) 
			{
			case ICEEnergyBarDataType.Age:
				return m_Adapter.Controller.Creature.Status.Age;

			case ICEEnergyBarDataType.Fitness:
				return m_Adapter.Controller.Creature.Status.FitnessInPercent;
			case ICEEnergyBarDataType.Health:
				return m_Adapter.Controller.Creature.Status.HealthInPercent;
			case ICEEnergyBarDataType.Armor:
				return m_Adapter.Controller.Creature.Status.ArmorInPercent;
			case ICEEnergyBarDataType.Stamina:
				return m_Adapter.Controller.Creature.Status.StaminaInPercent;
			case ICEEnergyBarDataType.Power:
				return m_Adapter.Controller.Creature.Status.PowerInPercent;


				case ICEEnergyBarDataType.Damage:
					return m_Adapter.Controller.Creature.Status.DamageInPercent;
			case ICEEnergyBarDataType.Stress:
				return m_Adapter.Controller.Creature.Status.StressInPercent;
			case ICEEnergyBarDataType.Debility:
				return m_Adapter.Controller.Creature.Status.DebilityInPercent;
			case ICEEnergyBarDataType.Hunger:
				return m_Adapter.Controller.Creature.Status.HungerInPercent;
			case ICEEnergyBarDataType.Thirst:
				return m_Adapter.Controller.Creature.Status.ThirstInPercent;

			case ICEEnergyBarDataType.SenseVisual:
				return m_Adapter.Controller.Creature.Status.SenseVisualInPercent;
			case ICEEnergyBarDataType.SenseOlfactory:
				return m_Adapter.Controller.Creature.Status.SenseOlfactoryInPercent;
			case ICEEnergyBarDataType.SenseGustatory:
				return m_Adapter.Controller.Creature.Status.SenseGustatoryInPercent;
			case ICEEnergyBarDataType.SenseAuditory:
				return m_Adapter.Controller.Creature.Status.SenseAuditoryInPercent;
			case ICEEnergyBarDataType.SenseTactile:
				return m_Adapter.Controller.Creature.Status.SenseTactileInPercent;

			case ICEEnergyBarDataType.Aggressivity:
				return m_Adapter.Controller.Creature.Status.AggressivityInPercent;
			case ICEEnergyBarDataType.Anxiety:
				return m_Adapter.Controller.Creature.Status.AnxietyInPercent;
			case ICEEnergyBarDataType.Experience:
				return m_Adapter.Controller.Creature.Status.ExperienceInPercent;
			case ICEEnergyBarDataType.Nosiness:
				return m_Adapter.Controller.Creature.Status.NosinessInPercent;

			}

			return 0;
		}

		private float GetMaxValue( ICEEnergyBarDataType _type )
		{
			switch( _type ) 
			{
				case ICEEnergyBarDataType.Age:
					return m_Adapter.Controller.Creature.Status.MaxAge;
			}

			return 100;
		}
	}

	[RequireComponent (typeof (ICECreatureControl))]
	public class ICECreatureEBTAdapter : MonoBehaviour 
	{
		private ICECreatureControl m_Controller = null;
		public Canvas Display = null;
		public float LineOffset = 0;
		public float InitialOffset = 2;

		public List<ICEEnergyBarObject> Bars = new List<ICEEnergyBarObject>();

		public ICECreatureControl Controller {
			get{
				if( m_Controller == null )
					m_Controller = transform.GetComponent<ICECreatureControl>();
			
				return m_Controller;
			}
		}

		void Awake(){
			Init();
		}

		private void Init() {
			foreach( ICEEnergyBarObject _bar in Bars )
				_bar.Init( this );
		}

		void OnEnable() {
			foreach( ICEEnergyBarObject _bar in Bars )
				_bar.EnableEnergyBar( this );
		}

		void OnDisable() {
			foreach( ICEEnergyBarObject _bar in Bars )
				_bar.DisableEnergyBar();
		}

		void OnDestroy() {
			foreach( ICEEnergyBarObject _bar in Bars )
				_bar.DestroyEnergyBar();
		}

		void Update(){
			foreach( ICEEnergyBarObject _bar in Bars )
				_bar.UpdateEnergyBar();
		}
	}
}