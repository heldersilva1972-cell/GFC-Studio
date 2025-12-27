// [NEW]
import { Controller } from 'react-hook-form';

export const DateSplitInput = ({ control, name, required }) => {
  return (
    <Controller
      name={name}
      control={control}
      rules={{ required }}
      render={({ field }) => (
        <div className="flex gap-2">
          <select
            value={field.value?.split('-')[1] || ''}
            onChange={(e) => {
              const month = e.target.value;
              const day = field.value?.split('-')[2] || '';
              const year = field.value?.split('-')[0] || '';
              field.onChange(`${year}-${month}-${day}`);
            }}
            required={required}
            className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
          >
            <option value="">Month</option>
            {[...Array(12)].map((_, i) => <option key={i} value={(i + 1).toString().padStart(2, '0')}>{new Date(0, i).toLocaleString('default', { month: 'long' })}</option>)}
          </select>
          <select
            value={field.value?.split('-')[2] || ''}
            onChange={(e) => {
                const month = field.value?.split('-')[1] || '';
                const day = e.target.value;
                const year = field.value?.split('-')[0] || '';
                field.onChange(`${year}-${month}-${day}`);
            }}
            required={required}
            className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
          >
            <option value="">Day</option>
            {[...Array(31)].map((_, i) => <option key={i} value={(i + 1).toString().padStart(2, '0')}>{i + 1}</option>)}
            </select>
          <select
            value={field.value?.split('-')[0] || ''}
            onChange={(e) => {
                const month = field.value?.split('-')[1] || '';
                const day = field.value?.split('-')[2] || '';
                const year = e.target.value;
                field.onChange(`${year}-${month}-${day}`);
            }}
            required={required}
            className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
          >
            <option value="">Year</option>
            {Array.from({ length: 100 }, (_, i) => new Date().getFullYear() - 21 - i).map(year => <option key={year} value={year}>{year}</option>)}
          </select>
        </div>
      )}
    />
  );
};
