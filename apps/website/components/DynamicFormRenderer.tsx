'use client';
// [NEW]

import { useState, useEffect } from 'react';

const DynamicFormRenderer = ({ formId }) => {
  const [form, setForm] = useState(null);
  const [formData, setFormData] = useState({});
  const [submissionStatus, setSubmissionStatus] = useState(null);

  useEffect(() => {
    const fetchForm = async () => {
      const res = await fetch(`/api/forms/${formId}`);
      const data = await res.json();
      setForm(data);
    };

    fetchForm();
  }, [formId]);

  const sanitizeLabel = (label) => {
    return label.toLowerCase().replace(/\s+/g, '-').replace(/[^a-z0-9-]/g, '');
  };

  const handleChange = (e) => {
    const { type, checked, files } = e.target;
    const label = e.target.dataset.label;
    const value = type === 'file' ? files[0] : e.target.value;

    setFormData((prevData) => ({
      ...prevData,
      [label]: type === 'checkbox' ? checked : value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const formDataToSubmit = new FormData();
    formDataToSubmit.append('formId', form.id);

    const jsonData = {};
    for (const key in formData) {
      if (formData[key] instanceof File) {
        formDataToSubmit.append(key, formData[key]);
      } else {
        jsonData[key] = formData[key];
      }
    }
    formDataToSubmit.append('submissionData', JSON.stringify(jsonData));

    const res = await fetch(`/api/forms/${form.id}/submit`, {
      method: 'POST',
      body: formDataToSubmit,
    });

    if (res.ok) {
      setSubmissionStatus('success');
      setFormData({});
    } else {
      setSubmissionStatus('error');
    }
  };

  if (!form) {
    return <div>Loading form...</div>;
  }

  return (
    <form onSubmit={handleSubmit}>
      <h3>{form.name}</h3>
      <p>{form.description}</p>
      {form.formFields.sort((a, b) => a.order - b.order).map((field) => (
        <div key={field.id} className="form-group">
          <label htmlFor={sanitizeLabel(field.label)}>{field.label}</label>
          {field.fieldType === 'text' && (
            <input
              type="text"
              id={sanitizeLabel(field.label)}
              name={sanitizeLabel(field.label)}
              data-label={field.label}
              placeholder={field.placeholder}
              required={field.isRequired}
              onChange={handleChange}
            />
          )}
          {field.fieldType === 'email' && (
            <input
              type="email"
              id={sanitizeLabel(field.label)}
              name={sanitizeLabel(field.label)}
              data-label={field.label}
              placeholder={field.placeholder}
              required={field.isRequired}
              onChange={handleChange}
            />
          )}
          {field.fieldType === 'tel' && (
            <input
              type="tel"
              id={sanitizeLabel(field.label)}
              name={sanitizeLabel(field.label)}
              data-label={field.label}
              placeholder={field.placeholder}
              required={field.isRequired}
              onChange={handleChange}
            />
          )}
          {field.fieldType === 'date' && (
            <input
              type="date"
              id={sanitizeLabel(field.label)}
              name={sanitizeLabel(field.label)}
              data-label={field.label}
              required={field.isRequired}
              onChange={handleChange}
            />
          )}
          {field.fieldType === 'select' && (
            <select
              id={sanitizeLabel(field.label)}
              name={sanitizeLabel(field.label)}
              data-label={field.label}
              required={field.isRequired}
              onChange={handleChange}
            >
              <option value="">{field.placeholder || 'Select...'}</option>
              {field.options && field.options.split(',').map((option) => (
                <option key={option.trim()} value={option.trim()}>
                  {option.trim()}
                </option>
              ))}
            </select>
          )}
          {field.fieldType === 'checkbox' && (
            <input
              type="checkbox"
              id={sanitizeLabel(field.label)}
              name={sanitizeLabel(field.label)}
              data-label={field.label}
              required={field.isRequired}
              onChange={handleChange}
            />
          )}
          {field.fieldType === 'file' && (
            <input
              type="file"
              id={sanitizeLabel(field.label)}
              name={sanitizeLabel(field.label)}
              data-label={field.label}
              required={field.isRequired}
              onChange={handleChange}
            />
          )}
        </div>
      ))}
      <button type="submit">Submit</button>
      {submissionStatus === 'success' && (
        <div className="alert alert-success">Form submitted successfully!</div>
      )}
      {submissionStatus === 'error' && (
        <div className="alert alert-danger">
          There was an error submitting the form.
        </div>
      )}
    </form>
  );
};

export default DynamicFormRenderer;
