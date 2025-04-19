using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SeeSharp.Admin.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<CategoryResponse>("https://localhost:7222/api/categories");
                return response?.Items.Select(item => new CategoryDto
                {
                    Id = item.CategoryId,
                    Name = item.CategoryName,
                    ImageUrl = item.ImageUrl,
                    IsActive = item.CategoryStatus == "Available",
                    IsDefault = item.IsDefault
                }).ToList() ?? new List<CategoryDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching categories: {ex.Message}");
                return new List<CategoryDto>();
            }
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(string id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<CategoryDto>($"https://localhost:7222/api/categories/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching category: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateCategoryAsync(CategoryDto category)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7222/api/categories", category);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating category: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateCategoryAsync(CategoryDto category)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7222/api/categories/{category.Id}", category);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"https://localhost:7222/api/categories/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
                return false;
            }
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType)
        {
            try
            {
                // Create multipart form content
                var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(imageStream);
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                content.Add(streamContent, "file", fileName);

                // Send the request
                var response = await _httpClient.PostAsync("https://localhost:7222/api/upload/image", content);
                
                if (response.IsSuccessStatusCode)
                {
                    // Assuming the API returns the URL of the uploaded image
                    var result = await response.Content.ReadFromJsonAsync<UploadResult>();
                    return result?.Url ?? string.Empty;
                }
                
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return string.Empty;
            }
        }
    }

    public class CategoryDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
    }

    public class CategoryResponse
    {
        public List<CategoryItem> Items { get; set; } = new List<CategoryItem>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    public class CategoryItem
    {
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string CategoryStatus { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class UploadResult
    {
        public string Url { get; set; } = string.Empty;
    }
} 