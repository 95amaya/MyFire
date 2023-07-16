<div class="card lg:card-side bg-base-100 shadow-xl h-full">
  <div class="card-body">
    <div class="card-title mx-auto text-3xl">
      <slot name="card-title">
        <p>Title Goes Here</p>
      </slot>
    </div>
    <slot name="card-body">
      <p>Body Goes Here</p>
    </slot>
  </div>
</div>
