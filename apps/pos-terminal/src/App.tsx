import { useState, useEffect } from 'react';
import { Database, Wifi, Settings, ShoppingCart, User, Plus, Minus, Trash2, Box, ArrowLeftRight, CheckCircle2 } from 'lucide-react';

interface LiquorItem {
  id: number;
  name: string;
  currentStock: number;
  bottleSize: string;
  category: string;
  retailPrice: number;
}

interface CartItem {
  id: number;
  name: string;
  price: number;
  quantity: number;
}

function App() {
  const [isOnline, setIsOnline] = useState(navigator.onLine);
  const [cart, setCart] = useState<CartItem[]>([]);
  const [isInventoryMode, setIsInventoryMode] = useState(false);
  const [liquorItems, setLiquorItems] = useState<LiquorItem[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [lastAction, setLastAction] = useState<string | null>(null);
  
  // Close Out Workflow State
  const [closeOutStep, setCloseOutStep] = useState<'prompt-inventory' | 'review-draft' | 'selection' | 'prompt-z' | null>(null);
  const [inventoryDraft, setInventoryDraft] = useState<Record<number, number>>({});

  // API URL - in development we'd use a proxy, but here we'll use the direct Blazor port
  const API_BASE = "http://localhost:5207/api/liquor";

  useEffect(() => {
    const handleOnline = () => setIsOnline(true);
    const handleOffline = () => setIsOnline(false);
    window.addEventListener('online', handleOnline);
    window.addEventListener('offline', handleOffline);
    
    fetchInventory();

    return () => {
      window.removeEventListener('online', handleOnline);
      window.removeEventListener('offline', handleOffline);
    };
  }, []);

  const fetchInventory = async () => {
    setIsLoading(true);
    try {
      const resp = await fetch(`${API_BASE}/items`);
      const data = await resp.json();
      setLiquorItems(data);
    } catch (err) {
      console.error("Failed to fetch inventory", err);
    } finally {
      setIsLoading(false);
    }
  };

  const handleBottleSwap = async (item: LiquorItem) => {
    try {
      const resp = await fetch(`${API_BASE}/checkout/${item.id}?userId=1&notes=POS Bottle Swap`, {
        method: 'POST'
      });
      
      if (resp.ok) {
        setLastAction(`-1 ${item.name}`);
        setLiquorItems(prev => prev.map(i => i.id === item.id ? { ...i, currentStock: i.currentStock - 1 } : i));
        setTimeout(() => setLastAction(null), 3000);
      }
    } catch (err) {
      console.error("Swap failed", err);
    }
  };

  const updateInventoryDraft = (id: number, delta: number) => {
    setInventoryDraft(prev => {
      const current = prev[id] || 0;
      const newVal = Math.max(0, current + delta);
      if (newVal === 0) {
        const next = { ...prev };
        delete next[id];
        return next;
      }
      return { ...prev, [id]: newVal };
    });
  };

  const commitCloseOut = async (printZ: boolean) => {
    // 1. Submit Inventory Draft if exists
    const pulls = Object.entries(inventoryDraft).map(([id, count]) => ({
      itemId: parseInt(id),
      count: count
    }));

    if (pulls.length > 0) {
      try {
        await fetch(`${API_BASE}/bulk-checkout?userId=1`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(pulls)
        });
      } catch (err) {
        console.error("Failed to commit inventory pulls", err);
      }
    }

    // 2. Perform Z-Report printing (Mock logic for now)
    if (printZ) {
      console.log("Printing Z-Report...");
      // In production: window.print() or thermal printing specific command
    }

    // 3. Reset POS
    setCart([]);
    setInventoryDraft({});
    setCloseOutStep(null);
    setLastAction("Shift Closed Successfully");
    setTimeout(() => setLastAction(null), 5000);
  };

  const addToCart = (item: LiquorItem) => {
    setCart(prev => {
      const existing = prev.find(i => i.id === item.id);
      if (existing) {
        return prev.map(i => i.id === item.id ? { ...i, quantity: i.quantity + 1 } : i);
      }
      return [...prev, { id: item.id, name: item.name, price: item.retailPrice, quantity: 1 }];
    });
  };

  const removeFromCart = (id: number) => {
    setCart(prev => prev.filter(i => i.id !== id));
  };

  const updateQuantity = (id: number, delta: number) => {
    setCart(prev => prev.map(i => {
      if (i.id === id) {
        const newQty = Math.max(1, i.quantity + delta);
        return { ...i, quantity: newQty };
      }
      return i;
    }));
  };

  const subtotal = cart.reduce((acc, item) => acc + (item.price * item.quantity), 0);

  return (
    <div className={`flex flex-col h-screen w-screen transition-colors duration-500 select-none overflow-hidden ${isInventoryMode ? 'bg-amber-950/20' : 'bg-slate-950'}`}>
      {/* Top Navbar */}
      <nav className={`flex items-center justify-between px-6 py-3 border-b z-50 transition-all ${isInventoryMode ? 'bg-amber-900/40 border-amber-500/30' : 'bg-slate-900/50 border-white/5 backdrop-blur-xl'}`}>
        <div className="flex items-center gap-3">
          <div className={`${isInventoryMode ? 'bg-amber-500' : 'bg-emerald-500'} p-2 rounded-xl shadow-lg transition-colors`}>
            {isInventoryMode ? <Box size={20} className="text-black" strokeWidth={2.5} /> : <Database size={20} className="text-white" strokeWidth={2.5} />}
          </div>
          <div>
            <h1 className="text-lg font-black tracking-tight leading-none uppercase text-white">GFC POS</h1>
            <span className={`text-[10px] font-bold uppercase tracking-widest leading-none ${isInventoryMode ? 'text-amber-500' : 'text-emerald-500'}`}>
              {isInventoryMode ? 'Inventory Mode' : 'Sales Terminal v1.0'}
            </span>
          </div>
        </div>
        
        <div className="flex items-center gap-6">
          {lastAction && (
             <div className="flex items-center gap-2 bg-amber-500 text-black px-4 py-1.5 rounded-full text-xs font-black animate-bounce">
                <CheckCircle2 size={14} />
                {lastAction}
             </div>
          )}

          <div className={`flex items-center gap-2 px-4 py-1.5 rounded-full text-xs font-bold uppercase tracking-wider transition-all border ${isOnline ? 'bg-emerald-500/10 text-emerald-400 border-emerald-500/20' : 'bg-rose-500/10 text-rose-400 border-rose-500/20'}`}>
            <span className={`w-2 h-2 rounded-full animate-pulse ${isOnline ? 'bg-emerald-400' : 'bg-rose-400'}`}></span>
            {isOnline ? 'Sync Active' : 'Offline'}
          </div>
          
          <div className="flex items-center gap-4">
            <button className="text-slate-400 hover:text-white transition-colors bg-white/5 p-2 rounded-full"><Settings size={18} /></button>
            <div className="h-6 w-[1px] bg-white/10"></div>
            <button 
              onClick={() => setCloseOutStep(Object.keys(inventoryDraft).length > 0 ? 'review-draft' : 'prompt-inventory')}
              className="group flex items-center gap-2 bg-emerald-500 hover:bg-emerald-400 text-white px-5 py-2 rounded-full border border-emerald-400/20 shadow-lg shadow-emerald-500/10 active:scale-95 transition-all"
            >
              <User size={16} className="group-hover:rotate-12 transition-transform" />
              <span className="text-xs font-black uppercase tracking-tight">Close Shift Out</span>
            </button>
          </div>
        </div>
      </nav>

      {/* Main Container */}
      <main className="flex-1 flex overflow-hidden relative">
        {/* Close Out Wizard - LIGHT THEME OVERLAY */}
        {closeOutStep && (
          <div className="absolute inset-0 z-[100] bg-white flex flex-col animate-in fade-in zoom-in duration-300">
            {/* Header */}
            <header className="px-8 py-6 border-b flex items-center justify-between">
              <div className="flex items-center gap-4">
                <div className="bg-slate-100 p-3 rounded-2xl">
                  {closeOutStep === 'selection' ? <Box size={24} className="text-slate-900" /> : <User size={24} className="text-slate-900" />}
                </div>
                <div>
                  <h2 className="text-xl font-black text-slate-900 uppercase tracking-tight">Shift Close-Out</h2>
                  <p className="text-xs font-bold text-slate-400 uppercase tracking-widest">
                    Step {closeOutStep === 'prompt-z' ? '2' : '1'} of 2
                  </p>
                </div>
              </div>
              <button onClick={() => setCloseOutStep(null)} className="text-slate-400 hover:text-slate-900 font-bold uppercase text-xs tracking-widest px-4 py-2">Cancel Close Out</button>
            </header>

            {/* Content Area */}
            <div className="flex-1 overflow-hidden p-8 flex flex-col items-center justify-center">
              
              {/* Step: Inventory Prompt */}
              {closeOutStep === 'prompt-inventory' && (
                <div className="max-w-xl w-full text-center space-y-8 animate-in slide-in-from-bottom-4">
                   <h3 className="text-4xl font-black text-slate-900 leading-tight text-center">Was any liquor taken from inventory?</h3>
                   <div className="grid grid-cols-2 gap-4">
                      <button 
                        onClick={() => setCloseOutStep('prompt-z')}
                        className="py-10 bg-slate-100 hover:bg-slate-200 text-slate-900 rounded-3xl font-black text-2xl uppercase tracking-widest transition-all"
                      >
                        No
                      </button>
                      <button 
                        onClick={() => setCloseOutStep('selection')}
                        className="py-10 bg-emerald-500 hover:bg-emerald-400 text-white rounded-3xl font-black text-2xl uppercase tracking-widest shadow-xl shadow-emerald-500/20 transition-all"
                      >
                        Yes
                      </button>
                   </div>
                </div>
              )}

              {/* Step: Review Draft */}
              {closeOutStep === 'review-draft' && (
                <div className="max-w-2xl w-full text-center space-y-8 animate-in slide-in-from-bottom-4">
                   <div className="space-y-4">
                     <h3 className="text-4xl font-black text-slate-900 leading-tight">Review Previous Selection</h3>
                     <p className="text-slate-500 font-bold uppercase tracking-widest text-xs">You previously selected bottles but did not finish close-out.</p>
                   </div>
                   
                   <div className="bg-slate-50 p-6 rounded-3xl border border-slate-100 space-y-2 max-h-48 overflow-y-auto">
                      {Object.entries(inventoryDraft).map(([id, count]) => {
                         const item = liquorItems.find(i => i.id === parseInt(id));
                         return (
                           <div key={id} className="flex justify-between items-center py-2 border-b border-slate-200 last:border-0">
                             <span className="font-bold text-slate-900 uppercase">{item?.name}</span>
                             <span className="bg-slate-900 text-white px-3 py-1 rounded-full font-black text-sm">x{count}</span>
                           </div>
                         );
                      })}
                   </div>

                   <p className="text-xl font-black text-slate-900">Is this selection still correct?</p>
                   
                   <div className="grid grid-cols-2 gap-4">
                      <button 
                        onClick={() => setCloseOutStep('prompt-z')}
                        className="py-8 bg-emerald-500 hover:bg-emerald-400 text-white rounded-3xl font-black text-xl uppercase tracking-widest shadow-xl shadow-emerald-500/20 transition-all"
                      >
                        Yes, Correct
                      </button>
                      <div className="grid grid-cols-1 gap-2">
                        <button 
                          onClick={() => setCloseOutStep('selection')}
                          className="py-4 bg-slate-100 hover:bg-slate-200 text-slate-900 rounded-2xl font-black text-sm uppercase tracking-widest transition-all"
                        >
                          No, Correct it
                        </button>
                        <button 
                          onClick={() => { setInventoryDraft({}); setCloseOutStep('prompt-inventory'); }}
                          className="py-4 bg-rose-50 hover:bg-rose-100 text-rose-500 rounded-2xl font-black text-sm uppercase tracking-widest transition-all"
                        >
                          Remove All
                        </button>
                      </div>
                   </div>
                </div>
              )}

              {/* Step: Selection Grid */}
              {closeOutStep === 'selection' && (
                <div className="h-full w-full flex flex-col gap-6 overflow-hidden">
                  <div className="flex items-center justify-between">
                    <h3 className="text-2xl font-black text-slate-900 uppercase">Select Bottles Removed</h3>
                    <div className="flex items-center gap-3">
                       <span className="text-xs font-black text-slate-400 uppercase tracking-widest">
                        {Object.values(inventoryDraft).reduce((a, b) => a + b, 0)} Items Selected
                       </span>
                       <button onClick={() => setInventoryDraft({})} className="text-xs font-black text-rose-500 uppercase hover:bg-rose-50 p-2 rounded-lg">Clear All</button>
                    </div>
                  </div>
                  
                  <div className="flex-1 overflow-y-auto pr-4">
                    <div className="grid grid-cols-2 lg:grid-cols-4 xl:grid-cols-6 gap-4">
                      {liquorItems.map(item => (
                        <button 
                          key={item.id}
                          onClick={() => updateInventoryDraft(item.id, 1)}
                          className={`relative p-6 border-2 rounded-3xl flex flex-col items-center gap-3 transition-all active:scale-[0.98] ${
                            inventoryDraft[item.id] 
                              ? 'bg-emerald-50 border-emerald-500 shadow-md' 
                              : 'bg-white border-slate-100 hover:border-slate-300'
                          }`}
                        >
                          <div className={`w-12 h-12 rounded-2xl flex items-center justify-center font-black text-white ${inventoryDraft[item.id] ? 'bg-emerald-500' : 'bg-slate-200'}`}>
                             {item.name[0]}
                          </div>
                          <span className="text-xs font-black text-slate-900 text-center leading-tight">{item.name}</span>
                          
                          {inventoryDraft[item.id] && (
                            <div className="absolute -top-3 -right-3 w-8 h-8 bg-emerald-500 text-white rounded-full flex items-center justify-center font-black text-sm shadow-lg border-4 border-white">
                              {inventoryDraft[item.id]}
                            </div>
                          )}

                          {inventoryDraft[item.id] && (
                            <button 
                              onClick={(e) => { e.stopPropagation(); updateInventoryDraft(item.id, -1); }}
                              className="absolute -bottom-2 bg-slate-900 text-white p-1 rounded-full shadow-lg"
                            >
                              <Minus size={12} />
                            </button>
                          )}
                        </button>
                      ))}
                    </div>
                  </div>

                  <div className="pt-6 border-t flex justify-between gap-4">
                    <button 
                      onClick={() => setCloseOutStep('prompt-inventory')}
                      className="px-10 py-5 bg-slate-100 hover:bg-slate-200 text-slate-900 rounded-2xl font-black uppercase tracking-widest transition-all"
                    >
                      Back
                    </button>
                    <button 
                      onClick={() => setCloseOutStep('prompt-z')}
                      className="flex-1 py-5 bg-emerald-500 hover:bg-emerald-400 text-white rounded-2xl font-black text-xl uppercase tracking-widest shadow-xl shadow-emerald-500/20 transition-all"
                    >
                      Continue to Z-Report
                    </button>
                  </div>
                </div>
              )}

              {/* Step: Z-Report Prompt */}
              {closeOutStep === 'prompt-z' && (
                <div className="max-w-xl w-full text-center space-y-8 animate-in slide-in-from-bottom-4">
                   <div className="space-y-2">
                     <h3 className="text-4xl font-black text-slate-900 leading-tight">Would you like to print the Z-Report?</h3>
                     <p className="text-slate-400 font-bold uppercase tracking-widest text-xs">This will finalize all inventory and sales data for this shift.</p>
                   </div>
                   <div className="grid grid-cols-1 gap-4">
                      <button 
                        onClick={() => commitCloseOut(true)}
                        className="py-8 bg-emerald-500 hover:bg-emerald-400 text-white rounded-3xl font-black text-2xl uppercase tracking-widest shadow-xl shadow-emerald-500/20 transition-all flex items-center justify-center gap-4"
                      >
                        <CheckCircle2 size={32} />
                        Yes, Print & Close
                      </button>
                      <button 
                        onClick={() => setCloseOutStep(null)}
                        className="py-6 bg-white hover:bg-rose-50 text-rose-500 border-2 border-rose-100 rounded-3xl font-black text-lg uppercase tracking-widest transition-all"
                      >
                        No, Cancel Close Out
                      </button>
                   </div>
                </div>
              )}

            </div>
          </div>
        )}

        {/* Left Side: Product Grid */}
        <section className="flex-1 p-6 overflow-y-auto pointer-events-auto">
          {isLoading && liquorItems.length === 0 ? (
            <div className="h-full flex items-center justify-center text-slate-500 font-bold uppercase tracking-widest">Loading Catalog...</div>
          ) : (
            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
              {liquorItems.map((item) => (
                <button 
                  key={item.id} 
                  onClick={() => addToCart(item)}
                  className="group relative flex flex-col items-center justify-between aspect-square p-5 bg-slate-900 border border-white/5 rounded-3xl hover:border-emerald-500/50 hover:bg-slate-800 transition-all active:scale-[0.98] shadow-sm hover:shadow-emerald-500/10"
                >
                  <div className={`w-14 h-14 bg-slate-700 rounded-2xl flex items-center justify-center text-white font-black text-xl shadow-lg shadow-black/40 group-hover:scale-105 transition-transform`}>
                    {item.name[0]}
                  </div>
                  
                  <div className="text-center">
                    <h3 className="font-bold text-sm tracking-tight text-white mb-1 uppercase">{item.name}</h3>
                    <div className="flex flex-col gap-1 items-center">
                      <span className="text-emerald-500 font-black text-lg">${item.retailPrice.toFixed(2)}</span>
                      <span className="text-[10px] text-slate-500 font-bold uppercase tracking-wide">{item.bottleSize}</span>
                    </div>
                  </div>
                </button>
              ))}
            </div>
          )}
        </section>

        {/* Right Side: Cart Sidebar */}
        <aside className="w-[400px] bg-slate-900/80 backdrop-blur-2xl border-l border-white/5 flex flex-col shadow-2xl z-40 transition-opacity duration-300">
          <div className="p-6 border-b border-white/5 flex items-center justify-between bg-white/2">
            <h2 className="flex items-center gap-3 font-black text-white text-xl uppercase tracking-tight">
              <ShoppingCart size={22} className="text-emerald-500" strokeWidth={2.5} />
              Active Tab
            </h2>
            <span className="text-xs font-black bg-emerald-500/10 text-emerald-500 border border-emerald-500/20 px-3 py-1 rounded-full uppercase tracking-widest">#0001</span>
          </div>
          
          <div className="flex-1 overflow-y-auto px-6 py-4 space-y-3">
            {cart.length === 0 ? (
              <div className="h-full flex flex-col items-center justify-center text-slate-500 p-8 text-center opacity-40">
                <div className="w-20 h-20 bg-white/5 rounded-full flex items-center justify-center mb-6">
                  <ShoppingCart size={32} />
                </div>
                <p className="font-black text-sm uppercase tracking-widest">Cart is Empty</p>
                <p className="text-xs mt-2 font-medium uppercase leading-loose">Tap items to the left<br/>to start this tab</p>
              </div>
            ) : (
              cart.map(item => (
                <div key={item.id} className="flex items-center justify-between bg-white/5 p-4 rounded-2xl border border-white/5 group">
                  <div className="flex-1">
                    <h4 className="font-bold text-sm text-white mb-1 uppercase tracking-tight">{item.name}</h4>
                    <span className="text-xs font-bold text-emerald-500">${(item.price * item.quantity).toFixed(2)}</span>
                  </div>
                  <div className="flex items-center gap-3">
                    <div className="flex items-center bg-black/40 rounded-xl p-1 border border-white/5">
                      <button onClick={() => updateQuantity(item.id, -1)} className="p-1.5 hover:text-white text-slate-400"><Minus size={14} /></button>
                      <span className="w-6 text-center font-black text-sm text-white">{item.quantity}</span>
                      <button onClick={() => updateQuantity(item.id, 1)} className="p-1.5 hover:text-white text-slate-400"><Plus size={14} /></button>
                    </div>
                    <button onClick={() => removeFromCart(item.id)} className="p-2 text-rose-500/40 hover:text-rose-500 hover:bg-rose-500/10 rounded-xl transition-all"><Trash2 size={16} /></button>
                  </div>
                </div>
              ))
            )}
          </div>

          <div className="p-8 bg-black/40 border-t border-white/10 space-y-6">
            <div className="space-y-2">
              <div className="flex justify-between text-slate-400 text-xs font-black uppercase tracking-widest">
                <span>Subtotal</span>
                <span>${subtotal.toFixed(2)}</span>
              </div>
              <div className="flex justify-between text-4xl font-black text-white tracking-tight">
                <span className="text-emerald-500">Total</span>
                <span>${subtotal.toFixed(2)}</span>
              </div>
            </div>
            <button 
              disabled={cart.length === 0}
              className="w-full py-5 bg-emerald-500 hover:bg-emerald-400 disabled:opacity-30 disabled:hover:bg-emerald-500 text-white rounded-2xl font-black shadow-xl shadow-emerald-500/20 active:scale-[0.97] transition-all text-xl uppercase tracking-widest"
            >
              Print & Pay
            </button>
          </div>
        </aside>
      </main>
    </div>
  );
}

export default App;
