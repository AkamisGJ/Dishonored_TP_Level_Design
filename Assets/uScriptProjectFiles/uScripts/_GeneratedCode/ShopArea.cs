//uScript Generated Code - Build 1.0.3109
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[NodePath("Graphs")]
[System.Serializable]
[FriendlyName("Untitled", "")]
public class ShopArea : uScriptLogic
{

   #pragma warning disable 414
   GameObject parentGameObject = null;
   uScript_GUI thisScriptsOnGuiListener = null; 
   bool m_RegisteredForEvents = false;
   
   //externally exposed events
   
   //external parameters
   
   //local nodes
   UnityEngine.Transform local_2_UnityEngine_Transform = default(UnityEngine.Transform);
   UnityEngine.GameObject local_3_UnityEngine_GameObject = default(UnityEngine.GameObject);
   UnityEngine.GameObject local_3_UnityEngine_GameObject_previous = null;
   UnityEngine.GameObject local_4_UnityEngine_GameObject = default(UnityEngine.GameObject);
   UnityEngine.GameObject local_4_UnityEngine_GameObject_previous = null;
   UnityEngine.GameObject local_6_UnityEngine_GameObject = default(UnityEngine.GameObject);
   UnityEngine.GameObject local_6_UnityEngine_GameObject_previous = null;
   UnityEngine.GameObject local_7_UnityEngine_GameObject = default(UnityEngine.GameObject);
   UnityEngine.GameObject local_7_UnityEngine_GameObject_previous = null;
   
   //owner nodes
   
   //logic nodes
   //pointer to script instanced logic node
   uScriptCon_CompareGameObjects logic_uScriptCon_CompareGameObjects_uScriptCon_CompareGameObjects_5 = new uScriptCon_CompareGameObjects( );
   UnityEngine.GameObject logic_uScriptCon_CompareGameObjects_A_5 = default(UnityEngine.GameObject);
   UnityEngine.GameObject logic_uScriptCon_CompareGameObjects_B_5 = default(UnityEngine.GameObject);
   System.Boolean logic_uScriptCon_CompareGameObjects_CompareByTag_5 = (bool) false;
   System.Boolean logic_uScriptCon_CompareGameObjects_CompareByName_5 = (bool) false;
   System.Boolean logic_uScriptCon_CompareGameObjects_ReportNull_5 = (bool) true;
   bool logic_uScriptCon_CompareGameObjects_Same_5 = true;
   bool logic_uScriptCon_CompareGameObjects_Different_5 = true;
   
   //event nodes
   UnityEngine.GameObject event_UnityEngine_GameObject_GameObject_0 = default(UnityEngine.GameObject);
   
   //property nodes
   
   //method nodes
   UnityEngine.Transform method_Detox_ScriptEditor_Plug_UnityEngine_GameObject_destination_1 = default(UnityEngine.Transform);
   System.Boolean method_Detox_ScriptEditor_Plug_UnityEngine_GameObject_isRunning_1 = (bool) false;
   #pragma warning restore 414
   
   //functions to refresh properties from entities
   
   void SyncUnityHooks( )
   {
      SyncEventListeners( );
      if ( null == local_2_UnityEngine_Transform || false == m_RegisteredForEvents )
      {
         GameObject gameObject = GameObject.Find( "Destination3" );
         if ( null != gameObject )
         {
            local_2_UnityEngine_Transform = gameObject.GetComponent<UnityEngine.Transform>();
         }
      }
      if ( null == local_3_UnityEngine_GameObject || false == m_RegisteredForEvents )
      {
         local_3_UnityEngine_GameObject = GameObject.Find( "T_Move" ) as UnityEngine.GameObject;
      }
      //if our game object reference was changed then we need to reset event listeners
      if ( local_3_UnityEngine_GameObject_previous != local_3_UnityEngine_GameObject || false == m_RegisteredForEvents )
      {
         //tear down old listeners
         if ( null != local_3_UnityEngine_GameObject_previous )
         {
            {
               uScript_Trigger component = local_3_UnityEngine_GameObject_previous.GetComponent<uScript_Trigger>();
               if ( null != component )
               {
                  component.OnEnterTrigger -= Instance_OnEnterTrigger_0;
                  component.OnExitTrigger -= Instance_OnExitTrigger_0;
                  component.WhileInsideTrigger -= Instance_WhileInsideTrigger_0;
               }
            }
         }
         
         local_3_UnityEngine_GameObject_previous = local_3_UnityEngine_GameObject;
         
         //setup new listeners
         if ( null != local_3_UnityEngine_GameObject )
         {
            {
               uScript_Trigger component = local_3_UnityEngine_GameObject.GetComponent<uScript_Trigger>();
               if ( null == component )
               {
                  component = local_3_UnityEngine_GameObject.AddComponent<uScript_Trigger>();
               }
               if ( null != component )
               {
                  component.OnEnterTrigger += Instance_OnEnterTrigger_0;
                  component.OnExitTrigger += Instance_OnExitTrigger_0;
                  component.WhileInsideTrigger += Instance_WhileInsideTrigger_0;
               }
            }
         }
      }
      if ( null == local_4_UnityEngine_GameObject || false == m_RegisteredForEvents )
      {
         local_4_UnityEngine_GameObject = GameObject.Find( "RigidBodyFPSController" ) as UnityEngine.GameObject;
      }
      //if our game object reference was changed then we need to reset event listeners
      if ( local_4_UnityEngine_GameObject_previous != local_4_UnityEngine_GameObject || false == m_RegisteredForEvents )
      {
         //tear down old listeners
         
         local_4_UnityEngine_GameObject_previous = local_4_UnityEngine_GameObject;
         
         //setup new listeners
      }
      //if our game object reference was changed then we need to reset event listeners
      if ( local_6_UnityEngine_GameObject_previous != local_6_UnityEngine_GameObject || false == m_RegisteredForEvents )
      {
         //tear down old listeners
         
         local_6_UnityEngine_GameObject_previous = local_6_UnityEngine_GameObject;
         
         //setup new listeners
      }
      if ( null == local_7_UnityEngine_GameObject || false == m_RegisteredForEvents )
      {
         local_7_UnityEngine_GameObject = GameObject.Find( "AI_Enemy" ) as UnityEngine.GameObject;
      }
      //if our game object reference was changed then we need to reset event listeners
      if ( local_7_UnityEngine_GameObject_previous != local_7_UnityEngine_GameObject || false == m_RegisteredForEvents )
      {
         //tear down old listeners
         
         local_7_UnityEngine_GameObject_previous = local_7_UnityEngine_GameObject;
         
         //setup new listeners
      }
   }
   
   void RegisterForUnityHooks( )
   {
      SyncEventListeners( );
      //if our game object reference was changed then we need to reset event listeners
      if ( local_3_UnityEngine_GameObject_previous != local_3_UnityEngine_GameObject || false == m_RegisteredForEvents )
      {
         //tear down old listeners
         if ( null != local_3_UnityEngine_GameObject_previous )
         {
            {
               uScript_Trigger component = local_3_UnityEngine_GameObject_previous.GetComponent<uScript_Trigger>();
               if ( null != component )
               {
                  component.OnEnterTrigger -= Instance_OnEnterTrigger_0;
                  component.OnExitTrigger -= Instance_OnExitTrigger_0;
                  component.WhileInsideTrigger -= Instance_WhileInsideTrigger_0;
               }
            }
         }
         
         local_3_UnityEngine_GameObject_previous = local_3_UnityEngine_GameObject;
         
         //setup new listeners
         if ( null != local_3_UnityEngine_GameObject )
         {
            {
               uScript_Trigger component = local_3_UnityEngine_GameObject.GetComponent<uScript_Trigger>();
               if ( null == component )
               {
                  component = local_3_UnityEngine_GameObject.AddComponent<uScript_Trigger>();
               }
               if ( null != component )
               {
                  component.OnEnterTrigger += Instance_OnEnterTrigger_0;
                  component.OnExitTrigger += Instance_OnExitTrigger_0;
                  component.WhileInsideTrigger += Instance_WhileInsideTrigger_0;
               }
            }
         }
      }
      //if our game object reference was changed then we need to reset event listeners
      if ( local_4_UnityEngine_GameObject_previous != local_4_UnityEngine_GameObject || false == m_RegisteredForEvents )
      {
         //tear down old listeners
         
         local_4_UnityEngine_GameObject_previous = local_4_UnityEngine_GameObject;
         
         //setup new listeners
      }
      //if our game object reference was changed then we need to reset event listeners
      if ( local_6_UnityEngine_GameObject_previous != local_6_UnityEngine_GameObject || false == m_RegisteredForEvents )
      {
         //tear down old listeners
         
         local_6_UnityEngine_GameObject_previous = local_6_UnityEngine_GameObject;
         
         //setup new listeners
      }
      //if our game object reference was changed then we need to reset event listeners
      if ( local_7_UnityEngine_GameObject_previous != local_7_UnityEngine_GameObject || false == m_RegisteredForEvents )
      {
         //tear down old listeners
         
         local_7_UnityEngine_GameObject_previous = local_7_UnityEngine_GameObject;
         
         //setup new listeners
      }
   }
   
   void SyncEventListeners( )
   {
   }
   
   void UnregisterEventListeners( )
   {
      if ( null != local_3_UnityEngine_GameObject )
      {
         {
            uScript_Trigger component = local_3_UnityEngine_GameObject.GetComponent<uScript_Trigger>();
            if ( null != component )
            {
               component.OnEnterTrigger -= Instance_OnEnterTrigger_0;
               component.OnExitTrigger -= Instance_OnExitTrigger_0;
               component.WhileInsideTrigger -= Instance_WhileInsideTrigger_0;
            }
         }
      }
   }
   
   public override void SetParent(GameObject g)
   {
      parentGameObject = g;
      
      logic_uScriptCon_CompareGameObjects_uScriptCon_CompareGameObjects_5.SetParent(g);
   }
   public void Awake()
   {
      
   }
   
   public void Start()
   {
      SyncUnityHooks( );
      m_RegisteredForEvents = true;
      
   }
   
   public void OnEnable()
   {
      RegisterForUnityHooks( );
      m_RegisteredForEvents = true;
   }
   
   public void OnDisable()
   {
      UnregisterEventListeners( );
      m_RegisteredForEvents = false;
   }
   
   public void Update()
   {
      
      //other scripts might have added GameObjects with event scripts
      //so we need to verify all our event listeners are registered
      SyncEventListeners( );
      
   }
   
   public void OnDestroy()
   {
   }
   
   void Instance_OnEnterTrigger_0(object o, uScript_Trigger.TriggerEventArgs e)
   {
      //fill globals
      event_UnityEngine_GameObject_GameObject_0 = e.GameObject;
      //relay event to nodes
      Relay_OnEnterTrigger_0( );
   }
   
   void Instance_OnExitTrigger_0(object o, uScript_Trigger.TriggerEventArgs e)
   {
      //fill globals
      event_UnityEngine_GameObject_GameObject_0 = e.GameObject;
      //relay event to nodes
      Relay_OnExitTrigger_0( );
   }
   
   void Instance_WhileInsideTrigger_0(object o, uScript_Trigger.TriggerEventArgs e)
   {
      //fill globals
      event_UnityEngine_GameObject_GameObject_0 = e.GameObject;
      //relay event to nodes
      Relay_WhileInsideTrigger_0( );
   }
   
   void Relay_OnEnterTrigger_0()
   {
      local_6_UnityEngine_GameObject = event_UnityEngine_GameObject_GameObject_0;
      {
         //if our game object reference was changed then we need to reset event listeners
         if ( local_6_UnityEngine_GameObject_previous != local_6_UnityEngine_GameObject || false == m_RegisteredForEvents )
         {
            //tear down old listeners
            
            local_6_UnityEngine_GameObject_previous = local_6_UnityEngine_GameObject;
            
            //setup new listeners
         }
      }
      Relay_In_5();
   }
   
   void Relay_OnExitTrigger_0()
   {
      local_6_UnityEngine_GameObject = event_UnityEngine_GameObject_GameObject_0;
      {
         //if our game object reference was changed then we need to reset event listeners
         if ( local_6_UnityEngine_GameObject_previous != local_6_UnityEngine_GameObject || false == m_RegisteredForEvents )
         {
            //tear down old listeners
            
            local_6_UnityEngine_GameObject_previous = local_6_UnityEngine_GameObject;
            
            //setup new listeners
         }
      }
   }
   
   void Relay_WhileInsideTrigger_0()
   {
      local_6_UnityEngine_GameObject = event_UnityEngine_GameObject_GameObject_0;
      {
         //if our game object reference was changed then we need to reset event listeners
         if ( local_6_UnityEngine_GameObject_previous != local_6_UnityEngine_GameObject || false == m_RegisteredForEvents )
         {
            //tear down old listeners
            
            local_6_UnityEngine_GameObject_previous = local_6_UnityEngine_GameObject;
            
            //setup new listeners
         }
      }
   }
   
   void Relay_SetDestination_1()
   {
      {
         {
            method_Detox_ScriptEditor_Plug_UnityEngine_GameObject_destination_1 = local_2_UnityEngine_Transform;
            
         }
         {
         }
      }
      {
         AI_Behaviour component;
         component = local_7_UnityEngine_GameObject.GetComponent<AI_Behaviour>();
         if ( null != component )
         {
            component.SetDestination(method_Detox_ScriptEditor_Plug_UnityEngine_GameObject_destination_1, method_Detox_ScriptEditor_Plug_UnityEngine_GameObject_isRunning_1);
         }
      }
   }
   
   void Relay_In_5()
   {
      {
         {
            {
               //if our game object reference was changed then we need to reset event listeners
               if ( local_6_UnityEngine_GameObject_previous != local_6_UnityEngine_GameObject || false == m_RegisteredForEvents )
               {
                  //tear down old listeners
                  
                  local_6_UnityEngine_GameObject_previous = local_6_UnityEngine_GameObject;
                  
                  //setup new listeners
               }
            }
            logic_uScriptCon_CompareGameObjects_A_5 = local_6_UnityEngine_GameObject;
            
         }
         {
            {
               //if our game object reference was changed then we need to reset event listeners
               if ( local_4_UnityEngine_GameObject_previous != local_4_UnityEngine_GameObject || false == m_RegisteredForEvents )
               {
                  //tear down old listeners
                  
                  local_4_UnityEngine_GameObject_previous = local_4_UnityEngine_GameObject;
                  
                  //setup new listeners
               }
            }
            logic_uScriptCon_CompareGameObjects_B_5 = local_4_UnityEngine_GameObject;
            
         }
         {
         }
         {
         }
         {
         }
      }
      logic_uScriptCon_CompareGameObjects_uScriptCon_CompareGameObjects_5.In(logic_uScriptCon_CompareGameObjects_A_5, logic_uScriptCon_CompareGameObjects_B_5, logic_uScriptCon_CompareGameObjects_CompareByTag_5, logic_uScriptCon_CompareGameObjects_CompareByName_5, logic_uScriptCon_CompareGameObjects_ReportNull_5);
      
      //save off values because, if there are multiple, our relay logic could cause them to change before the next value is tested
      bool test_0 = logic_uScriptCon_CompareGameObjects_uScriptCon_CompareGameObjects_5.Same;
      
      if ( test_0 == true )
      {
         Relay_SetDestination_1();
      }
   }
   
}
