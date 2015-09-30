using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.marcocarettoni.AutoCasterPro
{    
    class Spell
    {
        public int MANA_USE = 0;
        public String BUTTON = "";
        public Boolean WAIT_FULL_MANA = false;
        public int DELAY_MS = 0;

        public Spell()
        { }

        public Spell(String _MANA_USE, String _BUTTON, Boolean _WAIT_FULL_MANA)
        {
            MANA_USE = Int32.Parse(_MANA_USE);
            BUTTON = _BUTTON;
            WAIT_FULL_MANA = _WAIT_FULL_MANA;
        }

        public Spell(String _MANA_USE, String _BUTTON, Boolean _WAIT_FULL_MANA, String _DELAY_MS)
        {
            MANA_USE = Int32.Parse(_MANA_USE);
            BUTTON = _BUTTON;            
            DELAY_MS = Int32.Parse(_DELAY_MS);
            WAIT_FULL_MANA = _WAIT_FULL_MANA;
        }


    }
}
