import { useState, useEffect } from 'react';
import { Database, Wifi, WifiOff, Settings, ShoppingCart, User, Plus, Minus, Trash2 } from 'lucide-react';

interface MenuItem {
  id: number;
  name: string;
  price: number;
  category: string;
  color: string;
}

interface CartItem extends MenuItem {
  quantity: number;
}

const MENU_ITEMS: MenuItem[] = [
  { id: 1, name: 'Bud Light', price: 5.00, category: 'Beer', color: 'bg-blue-600' },
  { id: 2, name: 'Miller Lite', price: 5.00, category: 'Beer', color: 'bg-slate-600' },
  { id: 3, name: 'Jameson', price: 8.00, category: 'Liquor', color: 'bg-emerald-700' },
  { id: 4, name: 'Jack Daniels', price: 8.00, category: 'Liquor', color: 'bg-slate-800' },
  { id: 5, name: 'Tullamore Dew', price: 8.50, category: 'Liquor', color: 'bg-green-800' },
  { id: 6, name: 'Pinot Noir', price: 7.50, category: 'Wine', color: 'bg-rose-900' },
  { id: 7, name: 'Chardonnay', price: 7.50, category: 'Wine', color: 'bg-amber-100' },
  { id: 8, name: 'Soda / Water', price: 2.00, category: 'Non-Alc', color: 'bg-cyan-600' },
];

function App() {
  const [isOnline, setIsOnline] = useState(navigator.onLine);
  const [cart, setCart] = useState<CartItem[]>([]);
  
  useEffect(() => {
    const handleOnline = () => setIsOnline(true);
    const handleOffline = () => setIsOnline(false);
    window.addEventListener('online', handleOnline);
    window.addEventListener('offline', handleOffline);
    return () => {
      window.removeEventListener('online', handleOnline);
      window.removeEventListener('offline', handleOffline);
    };
  }, []);

  const addToCart = (item: MenuItem) => {
    setCart(prev => {
      const existing = prev.find(i => i.id === item.id);
      if (existing) {
        return prev.map(i => i.id === item.id ? { ...i, quantity: i.quantity + 1 } : i);
      }
      return [...prev, { ...item, quantity: 1 }];
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
    <div className="flex flex-col h-screen w-screen bg-slate-950 text-slate-100 select-none overflow-hidden">
      {/* Top Navbar */}
      <nav className="flex items-center justify-between px-6 py-3 bg-slate-900/50 backdrop-blur-xl border-b border-white/5 z-50">
        <div className="flex items-center gap-3">
          <div className="bg-emerald-500 p-2 rounded-xl shadow-lg shadow-emerald-500/20">
            <Database size={20} className="text-white" strokeWidth={2.5} />
          </div>
          <div>
            <h1 className="text-lg font-black tracking-tight leading-none uppercase">GFC POS</h1>
            <span className="text-[10px] text-emerald-500 font-bold uppercase tracking-widest leading-none">Terminal v1.0</span>
          </div>
        </div>
        
        <div className="flex items-center gap-6">
          <div className={`flex items-center gap-2 px-4 py-1.5 rounded-full text-xs font-bold uppercase tracking-wider transition-all border ${isOnline ? 'bg-emerald-500/10 text-emerald-400 border-emerald-500/20' : 'bg-rose-500/10 text-rose-400 border-rose-500/20'}`}>
            <span className={`w-2 h-2 rounded-full animate-pulse ${isOnline ? 'bg-emerald-400' : 'bg-rose-400'}`}></span>
            {isOnline ? 'Online Sync' : 'Offline Mode'}
          </div>
          <div className="flex items-center gap-4 text-slate-400">
            <button className="hover:text-white transition-colors bg-white/5 p-2 rounded-full"><Settings size={18} /></button>
            <div className="h-6 w-[1px] bg-white/10"></div>
            <button className="flex items-center gap-2 hover:text-white transition-colors bg-white/5 px-4 py-2 rounded-full border border-white/5">
              <User size={16} />
              <span className="text-xs font-bold uppercase tracking-tight">Admin</span>
            </button>
          </div>
        </div>
      </nav>

      {/* Main Container */}
      <main className="flex-1 flex overflow-hidden">
        {/* Left Side: Product Grid */}
        <section className="flex-1 p-6 overflow-y-auto bg-slate-950">
          <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
            {MENU_ITEMS.map((item) => (
              <button 
                key={item.id} 
                onClick={() => addToCart(item)}
                className="group relative flex flex-col items-center justify-between aspect-square p-5 bg-slate-900 border border-white/5 rounded-3xl hover:border-emerald-500/50 hover:bg-slate-800 transition-all active:scale-[0.98] shadow-sm hover:shadow-emerald-500/10"
              >
                <div className={`w-14 h-14 ${item.color} rounded-2xl flex items-center justify-center text-white font-black text-xl shadow-lg shadow-black/40 group-hover:scale-110 transition-transform`}>
                  {item.name[0]}
                </div>
                <div className="text-center">
                  <h3 className="font-bold text-sm tracking-tight text-white mb-1">{item.name}</h3>
                  <span className="text-emerald-500 font-black text-lg">${item.price.toFixed(2)}</span>
                </div>
              </button>
            ))}
          </div>
        </section>

        {/* Right Side: Cart Sidebar */}
        <aside className="w-[400px] bg-slate-900/80 backdrop-blur-2xl border-l border-white/5 flex flex-col shadow-2xl z-40">
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
                <p className="text-xs mt-2 font-medium opacity-60 uppercase leading-loose">Tap items to the left<br/>to start this tab</p>
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
