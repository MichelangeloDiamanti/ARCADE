using System.Collections;
using System.Collections.Generic;
using System;

namespace Condition
{
    public class ConditionManager {

        public ConditionValue Test(Condition c){
            
            ConditionValue status;
            switch(c.type)
            {
                case ConditionType.ISAT:
                    status = TestAT(c);
                    break;
                case ConditionType.HAS:
                    status = TestHAS(c);
                    break;
                default:
                    break;
            }

            return ConditionValue.TRUE;
        }

        public ConditionValue TestAT(Condition c){
            if( true /* whether c.ents[0] is at c.ents[1] */ ){
                return ConditionValue.TRUE;
            }
            else{
                return ConditionValue.FALSE;
            }
        }

        public ConditionValue TestHAS(Condition c){
            if( true /* whether c.ents[0] has c.ents[1] */ ){
                return ConditionValue.TRUE;
            }
            else{
                return ConditionValue.FALSE;
            }
        }

        public ConditionValue Do(Condition c, ConditionValue v){ // 'v' must be true or false
            
            ConditionValue status;
            switch(c.type)
            {
                case ConditionType.ISAT:
                    status = DoAT(c, v);
                    break;
                case ConditionType.HAS:
                    status = DoHAS(c, v);
                    break;
                default:
                    break;

            }

            //if status in pending, save in hashmap

            return ConditionValue.TRUE;
        }

        public ConditionValue DoAT(Condition c, ConditionValue v){
            
            return ConditionValue.TRUE;
            
        }

        public ConditionValue DoHAS(Condition c, ConditionValue v){
            
            return ConditionValue.TRUE;
            
        }

        ConditionManager(){
            Dictionary<Condition, ConditionValue> pendingConditions;

            // void CheckPendingConditions(){ // called every seconds or so
            //     // foreach( c,v in pending conditions){
            //     //      call Test(c);
            //     // }
            // } 

        }
    }
}


