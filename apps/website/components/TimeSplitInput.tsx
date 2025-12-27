// [NEW]
import { Controller } from 'react-hook-form';

export const TimeSplitInput = ({ control, name, required }) => {
  return (
    <Controller
      name={name}
      control={control}
      rules={{ required }}
      render={({ field }) => (
        <div className="flex gap-2">
          <select
            value={field.value?.split(':')[0] || ''}
            onChange={(e) => {
              const hour = e.target.value;
              const minute = field.value?.split(':')[1]?.split(' ')[0] || '00';
              const period = field.value?.split(' ')[1] || 'PM';
              field.onChange(`${hour}:${minute} ${period}`);
            }}
            required={required}
            className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
          >
            <option value="">Hour</option>
            {[...Array(12)].map((_, i) => <option key={i} value={i + 1}>{i + 1}</option>)}
          </select>
          <select
            value={field.value?.split(':')[1]?.split(' ')[0] || '00'}
            onChange={(e) => {
                const hour = field.value?.split(':')[0] || '1';
                const minute = e.target.value;
                const period = field.value?.split(' ')[1] || 'PM';
                field.onChange(`${hour}:${minute} ${period}`);
            }}
            required={required}
            className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
          >
            <option value="00">00</option>
            <option value="15">15</option>
            <option value="30">30</option>
            <option value="45">45</option>
            </select>
          <select
            value={field.value?.split(' ')[1] || 'PM'}
            onChange={(e) => {
                const hour = field.value?.split(':')[0] || '1';
                const minute = field.value?.split(':')[1]?.split(' ')[0] || '00';
                const period = e.target.value;
                field.onChange(`${hour}:${minute} ${period}`);
            }}
            required={required}
            className="w-1/3 bg-midnight-blue border border-white/20 rounded-md px-3 py-2 outline-none text-white"
          >
            <option value="AM">AM</option>
            <option value="PM">PM</option>
          </select>
        </div>
      )}
    />
  );
};
